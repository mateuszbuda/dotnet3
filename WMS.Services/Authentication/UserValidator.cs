using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.ServiceModel;
using WMS.DatabaseAccess;
using WMS.DatabaseAccess.Entities;
using WMS.ServicesInterface.DTOs;
using WMS.Services.Assemblers;
using WMS.ServicesInterface.DataContracts;

namespace WMS.Services.Authentication
{
    /// <summary>
    /// Klasa odpowiadająca za walidację użytkownika.
    /// </summary>
    public class UserValidator : UserNamePasswordValidator
    {
        public override void Validate(string userName, string password)
        {
            UserAssembler userAssembler = new UserAssembler();
            var u = userAssembler.ToEntity(new UserDto() { Username = userName, Password = password });
            User user = null;

            using (var context = new SystemContext())
            {
                context.TransactionSync(tc => 
                    user = tc.Entities.Users.Where(x => x.Username == u.Username).FirstOrDefault());
            }

            // makecert.exe -sr LocalMachine -ss My -a sha1 -n CN=TestCert -sky exchange -pe
            if (user == null || u.Password != user.Password)
                throw new FaultException("Zły login lub hasło!");
        }
    }
}