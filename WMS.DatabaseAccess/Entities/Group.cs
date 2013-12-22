using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace WMS.DatabaseAccess.Entities
{
    /// <summary>
    /// Klasa opisująca Partie.
    /// </summary>
    [Table("Group")]
    public class Group : Entity
    {
        /// <summary>
        /// Id sektora, w którym znajduje się partia.
        /// </summary>
        [Column("sector_id"), Required]
        public int SectorId { get; set; }

        /// <summary>
        /// Sektor, w którym znajduje się partia.
        /// </summary>
        [ForeignKey("SectorId")]
        public Sector Sector { get; set; }

        /// <summary>
        /// Wszystkie przesunięcia partii.
        /// </summary>
        public virtual ICollection<Shift> Shifts { get; set; }

        /// <summary>
        /// Szczegóły partii.
        /// </summary>
        public virtual ICollection<GroupDetails> GroupDetails { get; set; }
    }
}
