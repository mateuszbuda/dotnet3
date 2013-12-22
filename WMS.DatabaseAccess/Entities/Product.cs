using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace WMS.DatabaseAccess.Entities
{
    /// <summary>
    /// Klasa opisująca produkt
    /// </summary>
    public class Product : Entity
    {
        /// <summary>
        /// Nazwa produktu
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Data produkcji
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Cena
        /// </summary>
        [Required]
        public Decimal Price { get; set; }

        /// <summary>
        /// Szczegóły partii zawierających produkt
        /// </summary>
        public virtual ICollection<GroupDetails> GroupsDetails { get; set; }

        /// <summary>
        /// Krótki format daty.
        /// </summary>
        [NotMapped]
        public string DateShort { get { return Date.ToShortDateString(); } }
    }
}
