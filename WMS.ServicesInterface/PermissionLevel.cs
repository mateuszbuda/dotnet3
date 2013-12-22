using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.ServicesInterface
{
    /// <summary>
    /// Enum dla rodzajów kont użytkowników
    /// </summary>
    public enum PermissionLevel : int { Administrator = 0, Manager = 1, User = 2 }
}
