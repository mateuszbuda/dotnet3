using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace WMS.DatabaseAccess.Entities
{
    /// <summary>
    /// Klasa opisująca magazyn
    /// </summary>
    public class Warehouse : Entity
    {
        /// <summary>
        /// Flaga oznaczająca czy magazyn jest wewnętrzny
        /// </summary>
        [Required]
        public bool Internal { get; set; }

        /// <summary>
        /// Telefon
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// Adres email
        /// </summary>
        public string Mail { get; set; }

        /// <summary>
        /// Nazwa magazynu/partnera
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Ulica
        /// </summary>
        [Required]
        public string Street { get; set; }

        /// <summary>
        /// Numer budynku
        /// </summary>
        [Required]
        public string Num { get; set; }

        /// <summary>
        /// Miasto
        /// </summary>
        [Required]
        public string City { get; set; }

        /// <summary>
        /// Kod pocztowy
        /// </summary>
        [Required]
        public string Code { get; set; }

        /// <summary>
        /// Flaga oznaczająca czy magazyn jest usunięty
        /// </summary>
        [Required]
        public bool Deleted { get; set; }

        /// <summary>
        /// Sektory w magazynie
        /// </summary>
        public virtual ICollection<Sector> Sectors { get; set; }

        /// <summary>
        /// Wysłane partie
        /// </summary>
        public virtual ICollection<Shift> Sent { get; set; }

        ///// <summary>
        ///// Otrzymane partie
        ///// </summary>
        //public virtual ICollection<Shift> Received { get; set; }

        /// <summary>
        /// Partner (jeśli jest)
        /// </summary>
        public virtual ICollection<Partner> Owners { get; set; }
    }
}
