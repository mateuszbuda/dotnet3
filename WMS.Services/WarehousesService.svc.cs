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
    /// Serwis obsługujący zapytania związane z magazynami i sektorami
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerSession, IncludeExceptionDetailInFaults = true)]
    public class WarehousesService : ServiceBase, IWarehousesService
    {
        /// <summary>
        /// Zwraza listę wszystkich wewnętrznych i nieusuniętych magazynów
        /// </summary>
        /// <param name="request">Puste zapytanie</param>
        /// <returns>Odpowiedź z listą żądanych magazynów</returns>
        public Response<List<WarehouseDetailsDto>> GetWarehouses(Request request)
        {
            CheckPermissions(PermissionLevel.User);
            return new Response<List<WarehouseDetailsDto>>(request.Id,
                Transaction(tc => tc.Entities.Warehouses.Where(x => x.Internal && !x.Deleted).ToList().
                    Select(warehouseAssembler.ToSimpleDto).ToList()));
        }

        /// <summary>
        /// Zwraca listę magazynów partnerów.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Lista partnerów</returns>
        public Response<List<WarehouseDetailsDto>> GetPartnersWarehouses(Request request)
        {
            CheckPermissions(PermissionLevel.User);
            return new Response<List<WarehouseDetailsDto>>(request.Id,
                Transaction(tc => tc.Entities.Warehouses.Where(x => !x.Internal && !x.Deleted).ToList().
                    Select(warehouseAssembler.ToSimpleDto).ToList()));
        }

        /// <summary>
        /// Usuwa magazyn jeśli jest pusty
        /// </summary>
        /// <param name="warehouseId">Zapytanie z id magazynu do usunięcia</param>
        /// <returns>Odpowiedź z informację czy operacja się powiodła (czy magazyn był pusty)</returns>
        public Response<bool> DeleteIfEmpty(Request<int> warehouseId)
        {
            CheckPermissions(PermissionLevel.Administrator);
            bool ret = false;

            Transaction(tc =>
                {
                    var w = (from x in tc.Entities.Warehouses
                             where x.Id == warehouseId.Content
                             select x).FirstOrDefault();

                    int c = (from s in w.Sectors
                             where s.Deleted == false
                             where s.Groups.Count != 0
                             select s).Count();

                    if (c == 0)
                    {
                        ret = true;

                        w.Deleted = true;
                    }
                });

            return new Response<bool>(warehouseId.Id, ret);
        }

        /// <summary>
        /// Usuwa sektor jeśli jest pusty
        /// </summary>
        /// <param name="sectorId">Zapytanie z id sektora do usunięcia</param>
        /// <returns>Odpowiedź z informację czy operacja się powiodła (czy sektor był pusty)</returns>
        public Response<bool> DeleteSectorIfEmpty(Request<int> sectorId)
        {
            CheckPermissions(PermissionLevel.Administrator);
            bool ret = false;

            Transaction(tc =>
            {
                var s = (from x in tc.Entities.Sectors
                         where x.Id == sectorId.Content
                         select x).FirstOrDefault();

                int c = (from gr in s.Groups
                         select gr).Count();

                if (c == 0)
                {
                    ret = true;
                    s.Deleted = true;
                }
            });

            return new Response<bool>(sectorId.Id, ret);
        }

        /// <summary>
        /// Pobiera statyski związane z systemem
        /// </summary>
        /// <param name="request">Puste zapytanie</param>
        /// <returns>Odpowiedź ze statysykami</returns>
        public Response<StatisticsDto> GetStatistics(Request request)
        {
            CheckPermissions(PermissionLevel.User);
            StatisticsDto stats = new StatisticsDto();
            int all = 0;
            int full = 0;
            Transaction(tc =>
                {
                    stats.WarehousesCount = (from w in tc.Entities.Warehouses
                                             where w.Internal == true
                                             where w.Deleted == false
                                             select w).Count();
                    stats.ProductsCount = tc.Entities.Products.Count();
                    stats.PartnersCount = tc.Entities.Partners.Count();
                    stats.GroupsCount = (from g in tc.Entities.Groups
                                         where g.Sector.Warehouse.Internal == true
                                         select g).Count();
                    stats.ShiftsCount = tc.Entities.Shifts.Count();

                });
            Transaction(tc =>
                {
                    foreach (DatabaseAccess.Entities.Warehouse w in tc.Entities.Warehouses)
                        if (w.Internal == true && w.Deleted == false)
                            foreach (DatabaseAccess.Entities.Sector s in w.Sectors)
                            {
                                all += s.Limit;
                                full += s.Groups.Count;
                            }
                });

            stats.FIllRate = all == 0 ? 0 : (full * 100) / all;

            return new Response<StatisticsDto>(request.Id, stats);
        }

        /// <summary>
        /// Pobiera informacjie o zadanym przez id magazynie
        /// </summary>
        /// <param name="warehouseId">Zapytanie z id magazynu</param>
        /// <returns>Odpowiedź z żądanym magazynem</returns>
        public Response<WarehouseInfoDto> GetWarehouse(Request<int> warehouseId)
        {
            CheckPermissions(PermissionLevel.User);
            return new Response<WarehouseInfoDto>(warehouseId.Id, Transaction(tc =>
                tc.Entities.Warehouses.Where(x => x.Id == warehouseId.Content).Include(x => x.Sectors).
                Select(warehouseAssembler.ToDto).FirstOrDefault()));
        }

        /// <summary>
        /// Pobiera informacjie o magazynie w którym znajduje się grupa o zadanym id
        /// </summary>
        /// <param name="groupId">Zapytanie z id grupy</param>
        /// <returns>Odpowiedź z żądanym magazynem</returns>
        public Response<WarehouseInfoDto> GetWarehouseByGroup(Request<int> groupId)
        {
            CheckPermissions(PermissionLevel.User);
            int warehouseId = 0;
            Transaction(tc =>
                {
                    Group g = tc.Entities.Groups.Where(x => x.Id == groupId.Content).
                        Include(x => x.Sector).FirstOrDefault();
                    if (g == null)
                        throw new FaultException<ServiceException>(new ServiceException("Partia nie istnieje"));
                    warehouseId = g.Sector.WarehouseId;
                });
            return new Response<WarehouseInfoDto>(groupId.Id, Transaction(tc =>
                tc.Entities.Warehouses.Where(x => x.Id == warehouseId).Include(x => x.Sectors).
                Select(warehouseAssembler.ToDto).FirstOrDefault()));
        }

        /// <summary>
        /// Pobiera sektory dla zadanego magazynu
        /// </summary>
        /// <param name="warehouseId">Zapytanie z id magazynu</param>
        /// <returns>Odpowiedź z listą sektorów</returns>
        public Response<List<SectorDto>> GetSectors(Request<int> warehouseId)
        {
            CheckPermissions(PermissionLevel.User);
            return new Response<List<SectorDto>>(warehouseId.Id, Transaction(tc =>
                tc.Entities.Sectors.Where(s =>
                    (s.WarehouseId == warehouseId.Content && s.Deleted == false)).
                Include(x => x.Groups).Include(x => x.Warehouse).
                Select(sectorAssembler.ToDto).ToList()));
        }

        /// <summary>
        /// Pobiera informacje o zadanym sektorze
        /// </summary>
        /// <param name="sectorId">apytanie z id sektora</param>
        /// <returns>Odpowiedź z żądanym sektorem</returns>
        public Response<SectorDto> GetSector(Request<int> sectorId)
        {
            CheckPermissions(PermissionLevel.User);
            return new Response<SectorDto>(sectorId.Id, Transaction(tc =>
                tc.Entities.Sectors.Where(s => s.Id == sectorId.Content).
                Include(x => x.Groups).Include(x => x.Warehouse).
                Select(sectorAssembler.ToDto).FirstOrDefault()));
        }

        /// <summary>
        /// Dodaje nowy sektor
        /// </summary>
        /// <param name="sector">Zapytanie z sektorem do dodania</param>
        /// <returns>Odpowiedź z dodanym sektorem</returns>
        public Response<SectorDto> AddSector(Request<SectorDto> sector)
        {
            CheckPermissions(PermissionLevel.Administrator);
            Sector s = null;
            Transaction(tc =>
                {
                    s = tc.Entities.Sectors.Add(sectorAssembler.ToEntity(sector.Content));
                    if (s.Warehouse == null)
                        s.Warehouse = tc.Entities.Warehouses.Find(s.WarehouseId);
                });

            return new Response<SectorDto>(sector.Id, sectorAssembler.ToDto(s));
        }

        /// <summary>
        /// Dodaje nowy magazyn
        /// </summary>
        /// <param name="warehouse">Zapytanie z magazynem do dodania</param>
        /// <returns>Odpowiedź z dodanym magazynem</returns>
        public Response<WarehouseInfoDto> AddNew(Request<WarehouseInfoDto> warehouse)
        {
            CheckPermissions(PermissionLevel.Administrator);
            Warehouse w = null;
            Transaction(tc => w = tc.Entities.Warehouses.Add(warehouseAssembler.ToEntity(warehouse.Content)));
            return new Response<WarehouseInfoDto>(warehouse.Id, warehouseAssembler.ToDto(w));
        }

        /// <summary>
        /// Edycja danych o magazynie
        /// </summary>
        /// <param name="warehouse">Zapytanie z wyedytowanym magazynem</param>
        /// <returns>Odpowiedź z wyedytowanym magazynem</returns>
        public Response<WarehouseInfoDto> Edit(Request<WarehouseInfoDto> warehouse)
        {
            CheckPermissions(PermissionLevel.Manager);
            Warehouse w = null;

            Transaction(tc =>
                {
                    w = tc.Entities.Warehouses.Find(warehouse.Content.Id);
                    if (w == null)
                        throw new FaultException<ServiceException>(new ServiceException("Taki magazyn nie istnieje!"));

                    warehouseAssembler.ToEntity(warehouse.Content, w);
                });

            return new Response<WarehouseInfoDto>(warehouse.Id, warehouseAssembler.ToDto(w));
        }

        /// <summary>
        /// Zwraca numer kolejnego wolnego numeru sektora dla zadanego magazynu
        /// </summary>
        /// <param name="warehouseId">Zapytanie z id magazynu</param>
        /// <returns>Odpowiedź z kolejnym numerem wolnego sektora</returns>
        public Response<int> GetNextSectorNumber(Request<int> warehouseId)
        {
            CheckPermissions(PermissionLevel.Administrator);
            int ret = 1;

            Transaction(tc =>
                {
                    var w = tc.Entities.Warehouses.Find(warehouseId.Content);

                    if (w.Sectors.Count > 0)
                        ret = w.Sectors.Max(s => s.Number) + 1;
                });

            return new Response<int>(warehouseId.Id, ret);
        }

        /// <summary>
        /// Edycja istniejącego sektora
        /// </summary>
        /// <param name="sector">Zapytanie z wyedytowanym sektorem</param>
        /// <returns>Odpowiedź z wyedytowanym sektorem</returns>
        public Response<SectorDto> EditSector(Request<SectorDto> sector)
        {
            CheckPermissions(PermissionLevel.Manager);
            SectorDto ret = null;

            Transaction(tc =>
            {
                Sector s = tc.Entities.Sectors.Find(sector.Content.Id);
                if (s == null)
                    throw new FaultException<ServiceException>(new ServiceException("Taki sektor nie istnieje!"));

                sectorAssembler.ToEntity(sector.Content, s);

                if (s.Warehouse == null)
                    s.Warehouse = tc.Entities.Warehouses.Find(s.WarehouseId);

                ret = sectorAssembler.ToDto(s);
            });

            return new Response<SectorDto>(sector.Id, ret);
        }
    }
}
