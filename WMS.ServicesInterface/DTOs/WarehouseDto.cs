using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.ServicesInterface.DTOs
{
    /// <summary>
    /// Paczka z danymi ogólnymi na temat magazynu
    /// </summary>
    public class WarehouseInfoDto
    {
        /// <summary>
        /// Id magazynu
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Czy jest wewnętrznym magazynem czy zewnętrznym (partnera)
        /// </summary>
        public bool Internal { get; set; }
        /// <summary>
        /// Telefon kontaktowy do magazynu
        /// </summary>
        [Required]
        [StringLength(20)]
        [Display(Name = "Telefon")]
        public string Tel { get; set; }
        /// <summary>
        /// Mail do magazynu
        /// </summary>
        [Required]
        [StringLength(50)]
        [Display(Name = "Adres e-mail")]
        [DataType(DataType.EmailAddress, ErrorMessage = "To pole musi być poprawnym adresem e-mail")]
        [EmailAddress]
        public string Mail { get; set; }
        /// <summary>
        /// Nazwa magazynu
        /// </summary>
        [Required]
        [StringLength(30)]
        [Display(Name = "Nazwa magazynu")]
        public string Name { get; set; }
        /// <summary>
        /// Ulica magazynu
        /// </summary>
        [Required]
        [StringLength(50)]
        [Display(Name = "Ulica")]
        public string Street { get; set; }
        /// <summary>
        /// Numer lokalu magazynu
        /// </summary>
        [Required]
        [StringLength(8)]
        [Display(Name = "Numer")]
        public string Num { get; set; }
        /// <summary>
        /// Miasto w którym jest magazyn
        /// </summary>
        [Required]
        [StringLength(30)]
        [Display(Name = "Miasto")]
        public string City { get; set; }
        /// <summary>
        /// Kod pocztowy magazynu
        /// </summary>
        [Required]
        [StringLength(7)]
        [Display(Name = "Kod pocztowy")]
        public string Code { get; set; }
        /// <summary>
        /// Czy magazyn został usunięty
        /// </summary>
        public bool Deleted { get; set; }
        public byte[] Version { get; set; }
    }

    /// <summary>
    /// Paczka z informacjami o magazynie i jego strukturze
    /// </summary>
    public class WarehouseDetailsDto
    {
        /// <summary>
        /// Id magazynu
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Nazwa magazynu
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Czy jest wewnętrznym magazynem czy zewnętrznym (partnera)
        /// </summary>
        public bool Internal { get; set; }
        /// <summary>
        /// Liczba nieusuniętych sektorów w magazynie
        /// </summary>
        public int SectorsCount { get; set; }
        /// <summary>
        /// Liczba wolnych sektorów w magazynie
        /// </summary>
        public int FreeSectorsCount { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
