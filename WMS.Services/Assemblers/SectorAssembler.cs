using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using WMS.DatabaseAccess.Entities;
using WMS.ServicesInterface.DataContracts;
using WMS.ServicesInterface.DTOs;

namespace WMS.Services.Assemblers
{
    /// <summary>
    /// Klasa odpowiada za konwersję pomiędzy obiektami związanymi z sektorami
    /// mapowanymi do bazy a obiektami-paczkami przesyłanymi do i z serwera.
    /// </summary>
    public class SectorAssembler
    {
        /// <summary>
        /// Konwersja z sektora-paczki do komunikacji z serwerem na sektor bazodanowy.
        /// </summary>
        /// <param name="sector">Konwertowany sektor</param>
        /// <param name="ent">Edytowany bazodanowy sektor</param>
        /// <returns>Przekonwertowany sektor</returns>
        public Sector ToEntity(SectorDto sector, Sector ent = null)
        {
            if (ent != null && !sector.Version.SequenceEqual(ent.Version))
                throw new FaultException<ServiceException>(new ServiceException("Ktoś przed chwilą zmodyfikował dane.\nSpróbuj jeszcze raz."));

            ent = ent ?? new Sector();

            ent.Deleted = sector.Deleted;
            ent.Limit = sector.Limit;
            ent.Number = sector.Number;
            ent.WarehouseId = sector.WarehouseId;

            return ent;
        }

        /// <summary>
        /// Konwersja z bazodanowego sektora na sektor-paczkę do komunikacji z serwerem.
        /// </summary>
        /// <param name="sector">Konwertowany sektor</param>
        /// <returns>przekonwertowany sektor</returns>
        public SectorDto ToDto(Sector sector)
        {
            int gc = sector.Groups == null ? -1 : sector.Groups.Count;

            return new SectorDto
            {
                Deleted = sector.Deleted,
                GroupsCount = gc,
                Id = sector.Id,
                Limit = sector.Limit,
                Number = sector.Number,
                WarehouseId = sector.WarehouseId,
                WarehouseName = sector.Warehouse.Name,
                Version = sector.Version
            };
        }
    }
}