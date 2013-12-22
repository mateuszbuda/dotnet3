using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using WMS.DatabaseAccess;
using WMS.DatabaseAccess.Entities;
using WMS.ServicesInterface.DataContracts;
using WMS.ServicesInterface.DTOs;

namespace WMS.Services.Assemblers
{
    /// <summary>
    /// Klasa odpowiada za konwersję pomiędzy obiektami związanymi z magazynami
    /// mapowanymi do bazy a obiektami-paczkami przesyłanymi do i z serwera.
    /// </summary>
    public class WarehouseAssembler
    {
        /// <summary>
        /// Konwersja z bazodanowego magazynu na magazyn-paczkę do komunikacji z serwerem.
        /// Zawiera tylko podstawowe informacje dotyczące magazynu.
        /// </summary>
        /// <param name="warehouse">Konwertowany magazyn</param>
        /// <returns>przekonwertowany magazyn</returns>
        public WarehouseInfoDto ToDto(Warehouse warehouse)
        {
            return new WarehouseInfoDto
            {
                City = warehouse.City,
                Code = warehouse.Code,
                Deleted = warehouse.Deleted,
                Internal = warehouse.Internal,
                Mail = warehouse.Mail,
                Id = warehouse.Id,
                Name = warehouse.Name,
                Num = warehouse.Num,
                Street = warehouse.Street,
                Tel = warehouse.Tel,
                Version = warehouse.Version
            };
        }

        /// <summary>
        /// Konwersja z bazodanowego magazynu na magazyn-paczkę do komunikacji z serwerem.
        /// Zawiera dodatkowe informacje odtyczące zapełnienia magazynu.
        /// </summary>
        /// <param name="warehouse">Konwertowany magazyn</param>
        /// <returns>Przekonwertowany magazyn</returns>
        public WarehouseDetailsDto ToSimpleDto(Warehouse warehouse)
        {
            int fsc = 0;
            using (var context = new SystemContext())
            {
                context.TransactionSync(tc =>
                    fsc = tc.Entities.Warehouses.Where(x => x.Id == warehouse.Id).FirstOrDefault().
                    Sectors.Where(s => !s.Deleted).Where(s => s.Limit > s.Groups.Count).Count());
            }

            return new WarehouseDetailsDto
            {
                Id = warehouse.Id,
                Name = warehouse.Name,
                SectorsCount = warehouse.Sectors.Count,
                FreeSectorsCount = fsc,
                Internal = warehouse.Internal,
            };
        }

        /// <summary>
        /// Konwersja z magazynu-paczki do komunikacji z serwerem na magazyn bazodanowy.
        /// </summary>
        /// <param name="warehouse">Konwertowany magazyn</param>
        /// <param name="ent">Edytowany bazodanowy magazyn</param>
        /// <returns>Przekonwertowany magazyn</returns>
        public Warehouse ToEntity(WarehouseInfoDto warehouse, Warehouse ent = null)
        {
            if (ent != null && !warehouse.Version.SequenceEqual(ent.Version))
                throw new FaultException<ServiceException>(new ServiceException("Ktoś przed chwilą zmodyfikował dane.\nSpróbuj jeszcze raz."));

            ent = ent ?? new Warehouse();

            ent.City = warehouse.City;
            ent.Code = warehouse.Code;
            ent.Deleted = warehouse.Deleted;
            ent.Internal = warehouse.Internal;
            ent.Mail = warehouse.Mail;
            ent.Name = warehouse.Name;
            ent.Num = warehouse.Num;
            ent.Street = warehouse.Street;
            ent.Tel = warehouse.Tel;

            return ent;
        }
    }
}