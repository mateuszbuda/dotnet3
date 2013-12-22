﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.DatabaseAccess.Entities
{
    /// <summary>
    /// Główna klasa bazowa Encji.
    /// </summary>
    public abstract class Entity
    {
        /// <summary>
        /// Klucz główny.
        /// </summary>
        [Key, Required]
        public int Id { get; set; }

        /// <summary>
        /// Wersja rekordu.
        /// </summary>
        [Timestamp, ConcurrencyCheck]
        public byte[] Version { get; set; }
    }
}
