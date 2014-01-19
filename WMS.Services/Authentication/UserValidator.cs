using System;
using System.Linq;
using System.IdentityModel.Selectors;
using System.ServiceModel;
using WMS.DatabaseAccess;
using WMS.DatabaseAccess.Entities;
using WMS.ServicesInterface.DTOs;
using WMS.Services.Assemblers;

namespace WMS.Services.Authentication
{
    /// <summary>
    /// Klasa odpowiadająca za walidację użytkownika.
    /// </summary>
    public class UserValidator : UserNamePasswordValidator
    {
        public override void Validate(string userName, string password)
        {
            User u = new UserAssembler().ToEntity(new UserDto() { Username = userName, Password = password });

            using (var ctx = new SystemContext())
            {
                ctx.TransactionSync(tc =>
                {
                    User usr = tc.Entities.Users.Where(x => x.Username == userName && x.Password == u.Password).FirstOrDefault();

                    if (usr == null)
                        throw new FaultException("Zły login lub hasło");
                });
            }
        }
    }
}