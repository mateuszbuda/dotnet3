using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.ServicesInterface.DTOs
{
    /// <summary>
    /// Paczka z informacjami o urzytkownikach
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Nazwa użytkownika
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Hasło uzytkownika (hash)
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Id użytkownika
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Typ konta (okraśla prawa uzytkownika)
        /// </summary>
        public PermissionLevel Permissions { get; set; }

        public int PermissionsVal { get; set; }

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
