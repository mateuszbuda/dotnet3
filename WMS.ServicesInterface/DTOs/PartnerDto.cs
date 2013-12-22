using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.ServicesInterface.DTOs
{
    /// <summary>
    /// Paczka z danymi partnera
    /// </summary>
    public class PartnerSimpleDto
    {
        /// <summary>
        /// Id partenra
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Nazwa partnera
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Misto sidziby partnera
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// Kod pocztowy partnera
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Ulica siedziby partnera
        /// </summary>
        public string Street { get; set; }
        /// <summary>
        /// Numer lokalu siedziby partnera
        /// </summary>
        public string Num { get; set; }
        /// <summary>
        /// Telefon kontaktowy do partnera
        /// </summary>
        public string Tel { get; set; }
        /// <summary>
        /// E-mail partera
        /// </summary>
        public string Mail { get; set; }
        /// <summary>
        /// Wersja
        /// </summary>
        public byte[] Version { get; set; }
    }

    /// <summary>
    /// Paczka z dodatkowymi informacjami o magazynie partnera.
    /// </summary>
    public class PartnerDto : PartnerSimpleDto
    {
        /// <summary>
        /// Magazyn partera
        /// </summary>
        public WarehouseInfoDto Warehouse { get; set; }
    }
}
