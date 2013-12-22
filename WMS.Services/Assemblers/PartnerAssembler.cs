using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WMS.ServicesInterface.DTOs;
using WMS.DatabaseAccess.Entities;
using System.ServiceModel;
using WMS.ServicesInterface.DataContracts;

namespace WMS.Services.Assemblers
{
    /// <summary>
    /// Klasa odpowiada za konwersję pomiędzy obiektami związanymi z parterem
    /// mapowanymi do bazy a obiektami-paczkami przesyłanymi do i z serwera.
    /// </summary>
    public class PartnerAssembler
    {
        /// <summary>
        /// Konwersja z bazodanowego partnera na partnera-paczkę do komunikacji z serwerem.
        /// Nie uwzględnia informacji o magazynie partnera.
        /// </summary>
        /// <param name="partner">Konwertowany partner</param>
        /// <returns>Przekonwertowany partner</returns>
        public PartnerSimpleDto ToSimpleDto(Partner partner)
        {
            return new PartnerSimpleDto()
            {
                City = partner.City,
                Code = partner.Code,
                Id = partner.Id,
                Mail = partner.Mail,
                Name = partner.Warehouse.Name,
                Num = partner.Num,
                Street = partner.Street,
                Tel = partner.Tel,
                Version = partner.Version
            };
        }
        /// <summary>
        /// Konwersja z bazodanowego partnera na partnera-paczkę do komunikacji z serwerem.
        /// Zawiera informacje o magazynie partnera.
        /// </summary>
        /// <param name="partner">Konwertowany partner</param>
        /// <returns>Przekonwertowany partner</returns>
        public PartnerDto ToDto(Partner partner)
        {
            return new PartnerDto()
            {
                City = partner.City,
                Code = partner.Code,
                Id = partner.Id,
                Mail = partner.Mail,
                Name = partner.Warehouse.Name,
                Num = partner.Num,
                Street = partner.Street,
                Tel = partner.Tel,
                Warehouse = new WarehouseAssembler().ToDto(partner.Warehouse),
                Version = partner.Version
            };
        }

        /// <summary>
        /// Konwersja z partnera-paczki do komunikacji z serwerem na partnera bazodanowe.
        /// </summary>
        /// <param name="partner">Konwertowany partner</param>
        /// <param name="ent">Edytowany bazodanowy partner</param>
        /// <returns>Przekonwertowany partner</returns>
        public Partner ToEntity(PartnerDto partner, Partner ent = null)
        {
            
            if (ent != null && !partner.Version.SequenceEqual(ent.Version))
                throw new FaultException<ServiceException>(new ServiceException("Ktoś przed chwilą zmodyfikował dane.\nSpróbuj jeszcze raz."));

            ent = ent ?? new Partner();

            ent.City = partner.City;
            ent.Code = partner.Code;
            ent.Mail = partner.Mail;
            ent.Num = partner.Num;
            ent.Street = partner.Street;
            ent.Tel = partner.Tel;
            ent.Warehouse = new WarehouseAssembler().ToEntity(partner.Warehouse, ent.Warehouse);

            return ent;
        }
    }
}