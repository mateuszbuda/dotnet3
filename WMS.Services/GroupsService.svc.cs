using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WMS.ServicesInterface;
using WMS.ServicesInterface.ServiceContracts;
using WMS.ServicesInterface.DataContracts;
using WMS.ServicesInterface.DTOs;
using WMS.Services.Assemblers;
using WMS.DatabaseAccess.Entities;

namespace WMS.Services
{
    /// <summary>
    /// Servis obsługujący zapytania związane z partiami i przesunięciami
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerSession, IncludeExceptionDetailInFaults = true)]
    public class GroupsService : ServiceBase, IGroupsService
    {
        /// <summary>
        /// Pobiera partie, znajdujące się aktualnie w zadanym sektorze.
        /// </summary>
        /// <param name="sectorId">Zapytanie z id sektora dla którego pobieramy znajdujące się w nim partie</param>
        /// <returns>Odpowiedź z listą partii</returns>
        public Response<List<ShiftDto>> GetSectorGroups(Request<int> sectorId)
        {
            CheckPermissions(PermissionLevel.User);
            return new Response<List<ShiftDto>>(sectorId.Id, Transaction(tc =>
                tc.Entities.Shifts.Where(x => x.Latest && x.Group.SectorId == sectorId.Content).
                    Include(x => x.Group.Sector.Warehouse).Include(x => x.Sender).
                Select(groupAssembler.ToShiftDto).ToList()));
        }

        /// <summary>
        /// Pobiera informacja o zadanej grupie
        /// </summary>
        /// <param name="groupId">Zapytanie z id grupy, o której informacje chcemy pobrać</param>
        /// <returns>Odpowiedź z informacją o grupie</returns>
        public Response<GroupDto> GetGroupInfo(Request<int> groupId)
        {
            CheckPermissions(PermissionLevel.User);
            return new Response<GroupDto>(groupId.Id, Transaction(tc =>
                tc.Entities.Groups.Where(x => x.Id == groupId.Content).
                    Include(x => x.Sector.Warehouse).
                Select(groupAssembler.ToGroupDto).FirstOrDefault()));
        }

        /// <summary>
        /// Pobiera historię grupy jako listę przesunięć, jakie były wykonywane na tej grupie.
        /// </summary>
        /// <param name="groupId">Zapytanie z id grupy, o której informacje chcemy pobrać</param>
        /// <returns>Odpowiedź z informacją o grupie</returns>
        public Response<List<ShiftDto>> GetGroupHistory(Request<int> groupId)
        {
            CheckPermissions(PermissionLevel.User);
            return new Response<List<ShiftDto>>(groupId.Id, Transaction(tc =>
                tc.Entities.Shifts.Where(x => x.GroupId == groupId.Content).
                    Include(x => x.Group.Sector.Warehouse).Include(x => x.Sender).
                Select(groupAssembler.ToShiftDto).ToList()));
        }

        /// <summary>
        /// Pobiera wszystkie przesunięcie, któr są ostatnimi dla swoich partii.
        /// </summary>
        /// <param name="request">Puste zapytanie</param>
        /// <returns>Odpowiedź z listą przesunięć</returns>
        public Response<List<ShiftDto>> GetShifts(Request request)
        {
            CheckPermissions(PermissionLevel.User);
            return new Response<List<ShiftDto>>(request.Id, Transaction(tc =>
                tc.Entities.Shifts.Where(x => x.Latest).
                    Include(x => x.Group.Sector.Warehouse).Include(x => x.Sender).
                Select(groupAssembler.ToShiftDto).ToList()));
        }

        /// <summary>
        /// Pobiera informacje o produktach znajdujących się w partii.
        /// </summary>
        /// <param name="groupId">Zapytanie z id partii</param>
        /// <returns>Odpowiedź z listą szczegułów produktów przesyłanych w partii wraz z ich lościami</returns>
        public Response<List<ProductDetailsDto>> GetGroupDetails(Request<int> groupId)
        {
            CheckPermissions(PermissionLevel.User);
            return new Response<List<ProductDetailsDto>>(groupId.Id, Transaction(tc =>
                tc.Entities.GroupsDetails.Where(x => x.GroupId == groupId.Content).
                    Include(x => x.Product).
                Select(productAssembler.ToDetailsDto).ToList()));
        }

        /// <summary>
        /// Dodaje nowe przesunięcie i przesuwa partię aktualizując jej położenie
        /// i ustawiając ostatnie przesunięcie danej partii na bierzące.
        /// Jeśli jest to próba przesunięcia partii, która już została wydana
        /// do magazynu zewnętrznego (partnera) to rzucany jest wyjątek.
        /// </summary>
        /// <param name="shift">Zapytanie z przesunięciem, które ma być dodane</param>
        /// <returns>Odpowiedź z wykonanym przesunięciem</returns>
        public Response<bool> AddNewShift(Request<ShiftDto> shift)
        {
            CheckPermissions(PermissionLevel.User);
            // Sprawdzenie czy przesunięcie jest z magazynu wewnętrznego:
            Group g = null;
            Transaction(tc => g = tc.Entities.Shifts.
                Where(x => x.GroupId == shift.Content.GroupId && x.Latest).
                FirstOrDefault().Group);
            if (g != null && !g.Sector.Warehouse.Internal)
                throw new FaultException<ServiceException>(new ServiceException("Nie można przesunąć już wydanej grupy"));

            // Dodawanie przesunięcia:
            Shift s = null;
            Transaction(tc =>
                {
                    // Sprawdzenie czy przesuwana grupa istnieje:
                    Group group = tc.Entities.Groups.Where(x => x.Id == shift.Content.GroupId).FirstOrDefault();
                    if (group == null)
                        throw new FaultException<ServiceException>(new ServiceException("Nie ma takiej grupy :("));
                    //Sprawdzanie poprawności daty:
                    if (tc.Entities.Shifts.
                            Where(x => x.GroupId == shift.Content.GroupId && x.Date > shift.Content.Date).
                            ToList().Count > 0)
                        throw new FaultException<ServiceException>(new ServiceException("Data aktualnego przesunięcia jest wcześniejsza niż poprzedniego."));
                    // Sprawdzanie czy przesunięcie jest z magazynu wewnętrznego:
                    Shift shiftBefore = tc.Entities.Shifts.Where(x => x.GroupId == shift.Content.GroupId && x.Latest).
                        Include(x => x.Group.Sector.Warehouse).FirstOrDefault();
                    if (shiftBefore == null)
                        throw new FaultException<ServiceException>(new ServiceException("Wystąpił błąd podczas wykonywania przesunięcia :("));
                    if (!shiftBefore.Group.Sector.Warehouse.Internal)
                        throw new FaultException<ServiceException>(new ServiceException("Nie można wykonać przesunięcia parti z magazynu zewnętrznego (partnera)."));
                    // Aktualizacja ostatniego przesunięcia:
                    shiftBefore.Latest = false;
                    // Dodawanie przesunięcia, które zawsze musi być ostatnie:
                    //s.Latest = true;
                    s = tc.Entities.Shifts.Add(groupAssembler.ToShiftEntity(shift.Content));
                    // Aktualizacja lokalizacji grupy:
                    group.SectorId = shift.Content.RecipientSectorId;
                });
            return new Response<bool>(shift.Id, true);
        }

        /// <summary>
        /// Dodaje nową grupę wraz z pierwszym przesunięciem. Ilości produktów w tworzonej partii muszą być nieujemne,
        /// w przeciwnym przypadku rzucany jest wyjątek.
        /// </summary>
        /// <param name="group">Dodawana grupa</param>
        /// <returns>Dodaną grupa</returns>
        public Response<bool> AddNewGroup(Request<Tuple<GroupDetailsDto, ShiftDto>> newGroup)
        {
            CheckPermissions(PermissionLevel.User);

            GroupDetailsDto group = newGroup.Content.Item1;
            ShiftDto shift = newGroup.Content.Item2;

            Group g = null;
            Shift s = null;
            List<GroupDetails> gd = null;

            Transaction(tc =>
                {
                    // Pierwsze przesunięcie musi być od magazynu zewnętrznego (partnera)
                    // i i zarówno magazyn dostawcy jak i odbiorcy muszą być nieusunięte.
                    Warehouse sender = tc.Entities.Warehouses.Where(x => x.Id == shift.SenderId && !x.Deleted).FirstOrDefault();
                    Warehouse recipient = tc.Entities.Warehouses.Where(x => x.Id == shift.WarehouseId && !x.Deleted).FirstOrDefault();
                    if (sender == null || recipient == null)
                        throw new FaultException<ServiceException>(new ServiceException("Magazyn dostawcy lub odbiorcy nie istnieje lub został usunięty"));

                    if (sender.Internal)
                        throw new FaultException<ServiceException>(new ServiceException("Grupa nie moża przyjść z magazynu wewnętrznego"));

                    g = tc.Entities.Groups.Add(groupAssembler.ToGroupEntity(group));
                    gd = tc.Entities.GroupsDetails.AddRange(groupAssembler.ToGroupDetailsEntity(group)).ToList();
                    shift.GroupId = g.Id;
                    shift.Date = DateTime.Now;
                    s = tc.Entities.Shifts.Add(groupAssembler.ToShiftEntity(shift));

                    tc.Entities.SaveChanges();
                    g = tc.Entities.Groups.Where(x => x.Id == g.Id).Include(x => x.Sector.Warehouse).Include(x => x.GroupDetails).FirstOrDefault();
                });

            //return new Response<Tuple<GroupDetailsDto, ShiftDto>>(newGroup.Id,
               // new Tuple<GroupDetailsDto, ShiftDto>(groupAssembler.ToGroupDetailsDto(g), groupAssembler.ToShiftDto(s)));
            return new Response<bool>(newGroup.Id, true);
        }

        /// <summary>
        /// Sprawdzenie, czy nadawca przesunięcia jest magazynem wewnętrznym.
        /// </summary>
        /// <param name="shift">Zapytanie z przesunięciem do sprawdzenia</param>
        /// <returns>Odpowiedź true albo false</returns>
        public Response<bool> IsSenderInternal(Request<ShiftDto> shift)
        {
            CheckPermissions(PermissionLevel.User);
            bool ret = false;
            Transaction(tc =>
                {
                    var s = tc.Entities.Groups.Include(x => x.Shifts).Where(x => x.Id == shift.Content.Id).
                        FirstOrDefault().Shifts.Where(x => x.Latest == true).FirstOrDefault();
                    ret = tc.Entities.Warehouses.Find(s.SenderId).Internal;
                });
            return new Response<bool>(shift.Id, ret);
        }
    }
}
