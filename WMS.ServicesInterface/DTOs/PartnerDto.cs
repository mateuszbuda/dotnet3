using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

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
        [Required]
        [StringLength(30)]
        [Display(Name = "Nazwa partnara")]
        public string Name { get; set; }
        /// <summary>
        /// Misto sidziby partnera
        /// </summary>
        [Required]
        [StringLength(30)]
        [Display(Name = "Miasto")]
        public string City { get; set; }
        /// <summary>
        /// Kod pocztowy partnera
        /// </summary>
        [Required]
        [StringLength(7)]
        [Display(Name = "Kod pocztowy")]
        public string Code { get; set; }
        /// <summary>
        /// Ulica siedziby partnera
        /// </summary>
        [Required]
        [StringLength(50)]
        [Display(Name = "Ulica")]
        public string Street { get; set; }
        /// <summary>
        /// Numer lokalu siedziby partnera
        /// </summary>
        [Required]
        [StringLength(8)]
        [Display(Name = "Numer")]
        public string Num { get; set; }
        /// <summary>
        /// Telefon kontaktowy do partnera
        /// </summary>
        [Required]
        [StringLength(20)]
        [Display(Name = "Telefon")]
        public string Tel { get; set; }
        /// <summary>
        /// E-mail partera
        /// </summary>
        [Required]
        [StringLength(50)]
        [Display(Name = "Adres e-mail")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
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
