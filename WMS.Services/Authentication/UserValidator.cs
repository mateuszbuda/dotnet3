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
using System.Web.Security;

namespace WMS.Services.Authentication
{
    /// <summary>
    /// Klasa odpowiadająca za walidację użytkownika.
    /// </summary>
    public class UserValidator : UserNamePasswordValidator
    {
        public override void Validate(string userName, string password)
        {
            // Wyłączona walidacja!!!

            //FormsAuthenticationTicket ticket;
            //try
            //{
            //    ticket = FormsAuthentication.Decrypt(password);
            //}
            //catch
            //{
            //    throw new FaultException("Zły login lub hasło!");
            //}

            //if (ticket.Expired)
            //    throw new FaultException("Sesja wygasła!");

            //if (ticket.Name != userName)
            //    throw new FaultException("Zły login lub hasło");
        }
    }
}