using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WMS.ServicesInterface.DTOs
{
    /// <summary>
    /// Paczka z informacjami o użytkownikach
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Nazwa użytkownika
        /// </summary>
        /// 
        [Required]
        [StringLength(50)]
        [Display(Name = "Nazwa użytkownika")]
        public string Username { get; set; }
        /// <summary>
        /// Hasło uzytkownika (hash)
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }
        /// <summary>
        /// Hasło do potwierdzenia zgodności przy wprowadzaniu nowego lub edycji
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź hasło")]
        [Compare("Password", ErrorMessage = "Hasło i potwierdzenie nie są sobie równe.")]
        public string ConfirmPassword { get; set; }
        /// <summary>
        /// Id użytkownika
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Typ konta (okraśla prawa uzytkownika)
        /// </summary>
        [Display(Name = "Uprawnienia")]
        public PermissionLevel Permissions { get; set; }

        public int PermissionsVal { get; set; }

        public HttpCookie Token { get; set; }

        public bool Remember { get; set; }

        public string NewPassword { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is UserDto))
                return false;

            UserDto u = obj as UserDto;

            if (u.Id == this.Id && u.Password == this.Password && u.Permissions == this.Permissions
                && u.PermissionsVal == this.PermissionsVal && u.Username == this.Username)
                return true;

            return false;
        }
    }
}
