using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.ServicesInterface;
using WMS.ServicesInterface.ServiceContracts;
using WMS.ServicesInterface.DataContracts;
using WMS.ServicesInterface.DTOs;

namespace WMS.ServicesInterface.DTOs
{
    /// <summary>
    /// Paczka na informacje o sektorze
    /// </summary>
    public class SectorDto
    {
        /// <summary>
        /// Id sektora
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Numer sektora, unikalny w magazynie
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// Maksymalna liczba partii, które mogą być przechowywane w danym sektorze
        /// </summary>
        public int Limit { get; set; }
        /// <summary>
        /// Czy sektor został usunięty
        /// </summary>
        public bool Deleted { get; set; }
        /// <summary>
        /// Id magazynu, którego częścią jest sektor
        /// </summary>
        public int WarehouseId { get; set; }
        /// <summary>
        /// Nazwa magazynu, którego częścią jest sektor
        /// </summary>
        public string WarehouseName { get; set; }
        /// <summary>
        /// Ile partii jest aktualnie w sektorze
        /// </summary>
        public int GroupsCount { get; set; }
        /// <summary>
        /// Wersja
        /// </summary>
        public byte[] Version { get; set; }

        public override string ToString()
        {
            return "Sektor #" + Number.ToString();
        }
    }
}
