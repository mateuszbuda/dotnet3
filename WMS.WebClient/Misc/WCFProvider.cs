using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;
using WMS.ServicesInterface;
using WMS.ServicesInterface.DataContracts;
using WMS.ServicesInterface.ServiceContracts;
using WMS.WebClient.Models;

namespace WMS.WebClient.Misc
{
    public class WCFProvider : Controller
    {
        protected IWarehousesService WarehousesService { get; set; }
        protected IAuthenticationService AuthenticationService { get; set; }

        public WCFProvider()
        {
            ViewBag.Permissions = this.Permissions;
            string username = "";
            string password = "";

            try
            {
                username = System.Web.HttpContext.Current.User.Identity.Name;
                password = System.Web.HttpContext.Current.Request.Cookies["WMSSession"].Value;
            }
            catch { }
            //try

            // Wyłączenie autoryzacji!
            username = "admin";

            var authenticationChannelFactory = new ChannelFactory<IAuthenticationService>("SecureBinding_IAuthenticationService");
            authenticationChannelFactory.Credentials.UserName.UserName = username;
            //authenticationChannelFactory.Credentials.UserName.Password = "test";
            AuthenticationService = authenticationChannelFactory.CreateChannel();

            var warehouseChannelFactory = new ChannelFactory<IWarehousesService>("SecureBinding_IWarehousesService");
            warehouseChannelFactory.Credentials.UserName.UserName = username;
            warehouseChannelFactory.Credentials.UserName.Password = password;
            WarehousesService = warehouseChannelFactory.CreateChannel();
        }

        public ActionResult Execute<T>(Func<T> action, Func<Exception, ErrorMessage> errorMessage = null)
        {
            try
            {
                return View(action());
            }
            catch (Exception e)
            {
                if (errorMessage == null)
                    errorMessage = GetErrorMessage;

                return View("DataError", errorMessage(e));
            }
        }

        protected ErrorMessage GetErrorMessage(Exception e)
        {
            if (e.GetType() == typeof(FaultException<ServiceException>))
                return new ErrorMessage("Błąd", (e as FaultException<ServiceException>).Detail.Message);
            else if (e.GetType() == typeof(ClientException))
                return new ErrorMessage("Błąd", e.Message);
            else
                return new ErrorMessage("Błąd", "Nieznany błąd wewnętrzny serwera.");
        }

        public PermissionLevel Permissions
        {
            get
            {
                PermissionLevel ret = PermissionLevel.Unknown;

                var c = System.Web.HttpContext.Current.Request.Cookies["WMSPermissions"];

                if (c == null || c.Value == null)
                    System.Web.HttpContext.Current.Response.Cookies.Add(new HttpCookie("WMSSession") { Expires = DateTime.Now.AddDays(-1) });
                else
                    switch (c.Value)
                    {
                        case "Administrator":
                            ret = PermissionLevel.Administrator;
                            break;
                        case "Manager":
                            ret = PermissionLevel.Manager;
                            break;
                        case "User":
                            ret = PermissionLevel.User;
                            break;
                        default:
                            ret = PermissionLevel.Unknown;
                            break;
                    }

                return ret;
            }
        }
    }
}