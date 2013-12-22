﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace WMS.DatabaseAccess.Entities
{
    /// <summary>
    /// Klasa opisująca sektor.
    /// </summary>
    public class Sector : Entity
    {
        /// <summary>
        /// Id magazynu, w którym znajduje się sektor
        /// </summary>
        [Column("warehouse_id"), Required]
        public int WarehouseId { get; set; }

        /// <summary>
        /// Numer sektora w magazynie
        /// </summary>
        [Required]
        public int Number { get; set; }

        /// <summary>
        /// Maksymalna liczba partii przechowywanych w sektorzy.
        /// </summary>
        [Required]
        public int Limit { get; set; }

        /// <summary>
        /// Flaga wskazująca czy sektor jest usunięty
        /// </summary>
        [Required]
        public bool Deleted { get; set; }

        /// <summary>
        /// Magazyn, w którym znajduje się sektor
        /// </summary>
        [ForeignKey("WarehouseId")]
        public Warehouse Warehouse { get; set; }

        /// <summary>
        /// Partie w sektorze
        /// </summary>
        public virtual ICollection<Group> Groups { get; set; }
    }
}
