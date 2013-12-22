using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using WMS.DatabaseAccess;
using WMS.DatabaseAccess.Entities;
using WMS.ServicesInterface.DataContracts;
using WMS.ServicesInterface.DTOs;
using System.Data.Entity;

namespace WMS.Services.Assemblers
{
    /// <summary>
    /// Klasa odpowiada za konwersję pomiędzy obiektami związanymi z partią
    /// mapowanymi do bazy a obiektami-paczkami przesyłanymi do i z serwera.
    /// </summary>
    public class GroupAssembler
    {
        /// <summary>
        /// Konwersja z przesunięcia bazodanowego na przesunięcie-paczkę do komunikacji z serwerem
        /// przy pobieraniu historii przesunięć dla magazynu.
        /// </summary>
        /// <param name="shift">Konwertowane przesunięcie</param>
        /// <returns>Przekonwertowane przesunięcie</returns>
        public ShiftHistoryDto ToShiftHistoryDto(Shift shift)
        {
            return new ShiftHistoryDto()
            {
                Id = shift.GroupId,
                Date = shift.Date,
                RecipientId = shift.Group.Sector.Warehouse.Id,
                RecipientName = shift.Group.Sector.Warehouse.Name,
                SenderId = shift.Sender.Id,
                SenderName = shift.Sender.Name,
                Internal = shift.Group.Sector.Warehouse.Internal,
            };
        }

        /// <summary>
        /// Konwersja z przesunięcia bazodanowego na przesunięcie-paczkę do komunikacji z serwerem.
        /// </summary>
        /// <param name="shift">Konwertowane przesunięcie</param>
        /// <returns>Przekonwertowane przesunięcie</returns>
        public ShiftDto ToShiftDto(Shift shift)
        {
            int warehouseId = 0;
            string warehouseName = "";
            int groupId = shift.GroupId;

            if (shift.Latest)
            {
                warehouseId = shift.Group.Sector.WarehouseId;
                warehouseName = shift.Group.Sector.Warehouse.Name;
            }
            else
            {
                List<Shift> shifts;
                using (var context = new SystemContext())
                {
                    context.TransactionSync(tc =>
                        {
                            shifts = tc.Entities.Shifts.Include(x => x.Sender).Where(x => x.GroupId == groupId).ToList().
                                FindAll(x => x.Date > shift.Date);
                            shifts.Sort((x, y) =>
                                {
                                    if (x.Date < y.Date) return -1;
                                    else if (x.Date > y.Date) return 1;
                                    return 0;
                                });
                            warehouseId = shifts[0].SenderId;
                            warehouseName = shifts[0].Sender.Name;
                        });
                }
            }

            return new ShiftDto()
            {
                Date = shift.Date,
                Id = shift.GroupId,
                Internal = shift.Group.Sector.Warehouse.Internal,
                SenderId = shift.SenderId,
                SenderName = shift.Sender.Name,
                WarehouseId = warehouseId,
                WarehouseName = warehouseName,
                Version = shift.Version,
            };
        }

        /// <summary>
        /// Konwersja z partii bazodanowej na partii-paczki do komunikacji z serwerem.
        /// </summary>
        /// <param name="group">Konwertowana partia</param>
        /// <returns>Przekonwertowana partia</returns>
        public GroupDto ToGroupDto(Group group)
        {
            return new GroupDto()
            {
                Id = group.Id,
                Internal = group.Sector.Warehouse.Internal,
                SectorNumber = group.Sector.Number,
                WarehouseName = group.Sector.Warehouse.Name,
            };
        }

        /// <summary>
        /// Konwersja z partii bazodanowej do partii-paczki do komunikacji z serwerem
        /// z informacją o produktach w przesunięciu i ich ilościami.
        /// </summary>
        /// <param name="group">Konwertowana partia</param>
        /// <returns>Przekonwertowana partia</returns>
        //public GroupDetailsDto ToGroupDetailsDto(Group g)
        //{
        //    GroupDetailsDto newGroup = new GroupDetailsDto()
        //    {
        //        Id = g.Id,
        //        Internal = g.Sector.Warehouse.Internal,
        //        SectorId = g.SectorId,
        //        SectorNumber = g.Sector.Number,
        //        WarehouseName = g.Sector.Warehouse.Name,
        //    };
        //    newGroup.Products = new List<ProductDetailsDto>(g.GroupDetails.Count);
        //    foreach (GroupDetails gd in g.GroupDetails)
        //        newGroup.Products.Add(new ProductDetailsDto()
        //        {
        //            Count = gd.Count,
        //            Name = gd.Product.Name,
        //            Price = gd.Product.Price,
        //            ProductionDate = gd.Product.Date,
        //        });
        //    return newGroup;
        //}

        /// <summary>
        /// Konwersja z partii-paczki do komunikacji z serwerem na partię bazodanową
        /// z informacją o produktach w przesunięciu i ich ilościami.
        /// </summary>
        /// <param name="group">Konwertowana partia</param>
        /// <returns>Przekonwertowana partia</returns>
        public List<GroupDetails> ToGroupDetailsEntity(GroupDetailsDto groupDetails, List<GroupDetails> ents = null)
        {
            if (ents != null)
                foreach (GroupDetails ent in ents)
                    if (ent != null && !groupDetails.Version.SequenceEqual(ent.Version))
                        throw new FaultException<ServiceException>(new ServiceException("Ktoś przed chwilą zmodyfikował dane.\nSpróbuj jeszcze raz."));

            ents = ents ?? new List<GroupDetails>(groupDetails.Products.Count);

            foreach (ProductDetailsDto p in groupDetails.Products)
            {
                if (p.Count < 0)
                    throw new FaultException<ServiceException>(new ServiceException("Ilość produktów w przesunięciu musi być nieujemna."));

                ents.Add(new GroupDetails()
                {
                    Count = p.Count,
                    GroupId = groupDetails.Id,
                    ProductId = p.Id,
                });
            }

            return ents;
        }

        /// <summary>
        /// Konwersja z przesunięcia-paczki do komunikacji z serwerem na przesunięcie bazodanowe.
        /// </summary>
        /// <param name="shift">Konwertowane przesunięcie</param>
        /// <returns>Przekonwertowane przesunięcie</returns>
        public Shift ToShiftEntity(ShiftDto shift, Shift ent = null)
        {
            if (ent != null && !shift.Version.SequenceEqual(ent.Version))
                throw new FaultException<ServiceException>(new ServiceException("Ktoś przed chwilą zmodyfikował dane.\nSpróbuj jeszcze raz."));

            ent = ent ?? new Shift();

            ent.Date = shift.Date;
            ent.GroupId = shift.GroupId;
            ent.SenderId = shift.SenderId;

            int shiftsAfterCount = 0;
            using (var context = new SystemContext())
            {
                context.TransactionSync(tc =>
                    {
                        shiftsAfterCount = tc.Entities.Shifts.
                            Where(x => x.GroupId == shift.GroupId && x.Date > shift.Date).
                            ToList().Count;
                    });
            }

            ent.Latest = shiftsAfterCount <= 0;

            return ent;
        }

        /// <summary>
        /// Konwersja z przesunięcia-paczki do komunikacji z serwerem przy pobieraniu historii przesunięć dla magazynu
        /// na przesunięcie bazodanowe.
        /// </summary>
        /// <param name="group">Konwertowane przesunięcie</param>
        /// <param name="ent">Edytowane bazodanowe przesunięcie</param>
        /// <returns>Przekonwertowane przesunięcie</returns>
        public Group ToGroupEntity(GroupDto group, Group ent = null)
        {
            if (ent != null && !group.Version.SequenceEqual(ent.Version))
                throw new FaultException<ServiceException>(new ServiceException("Ktoś przed chwilą zmodyfikował dane.\nSpróbuj jeszcze raz."));

            ent = ent ?? new Group();

            ent.SectorId = group.SectorId;

            return ent;
        }
    }
}