using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.DatabaseAccess.Entities
{
    /// <summary>
    /// Klasa opisująca użytkownika
    /// </summary>
    public class User
    {
        /// <summary>
        /// Id
        /// </summary>
        [Key, Required]
        public int Id { get; set; }

        /// <summary>
        /// Nazwa użytkownika
        /// </summary>
        [Column("login"), Required]
        public string Username { get; set; }

        /// <summary>
        /// Hasło
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// Uprawnienia (0-2)
        /// </summary>
        [Required]
        public int Permissions { get; set; }
    }
}
