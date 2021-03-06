﻿using System;
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
        protected IProductsService ProductsService { get; set; }
        protected IAuthenticationService AuthenticationService { get; set; }
        protected IGroupsService GroupsService { get; set; }
        protected IPartnersService PartnersService { get; set; }
        protected IAdministrationService AdministrationService { get; set; }

        public WCFProvider()
        {
            ViewBag.Permissions = this.Permissions;
            string username = "";
            string password = "";

            try
            {
                username = System.Web.HttpContext.Current.User.Identity.Name;
                password = System.Web.HttpContext.Current.Session["WMSData"] as string;
            }
            catch { }
            //try

            try
            {
                var authenticationChannelFactory = new ChannelFactory<IAuthenticationService>("SecureBinding_IAuthenticationService");
                authenticationChannelFactory.Credentials.UserName.UserName = username;
                //authenticationChannelFactory.Credentials.UserName.Password = "test";
                AuthenticationService = authenticationChannelFactory.CreateChannel();

                var administrationChannelFactory = new ChannelFactory<IAdministrationService>("SecureBinding_IAdministrationService");
                administrationChannelFactory.Credentials.UserName.UserName = username;
                administrationChannelFactory.Credentials.UserName.Password = password;
                AdministrationService = administrationChannelFactory.CreateChannel();

                var productChannelFactory = new ChannelFactory<IProductsService>("SecureBinding_IProductsService");
                productChannelFactory.Credentials.UserName.UserName = username;
                productChannelFactory.Credentials.UserName.Password = password;
                ProductsService = productChannelFactory.CreateChannel();

                var warehouseChannelFactory = new ChannelFactory<IWarehousesService>("SecureBinding_IWarehousesService");
                warehouseChannelFactory.Credentials.UserName.UserName = username;
                warehouseChannelFactory.Credentials.UserName.Password = password;
                WarehousesService = warehouseChannelFactory.CreateChannel();

                var groupsChannelFactory = new ChannelFactory<IGroupsService>("SecureBinding_IGroupsService");
                groupsChannelFactory.Credentials.UserName.UserName = username;
                groupsChannelFactory.Credentials.UserName.Password = password;
                GroupsService = groupsChannelFactory.CreateChannel();

                var partnersChannelFactory = new ChannelFactory<IPartnersService>("SecureBinding_IPartnersService");
                partnersChannelFactory.Credentials.UserName.UserName = username;
                partnersChannelFactory.Credentials.UserName.Password = password;
                PartnersService = partnersChannelFactory.CreateChannel();
            }
            catch
            {
                System.Web.HttpContext.Current.Response.Cookies.Add(new HttpCookie("WMSSession") { Expires = DateTime.Now.AddDays(-1) });
                System.Web.HttpContext.Current.Response.Cookies.Add(new HttpCookie("WMSPermissions") { Expires = DateTime.Now.AddDays(-1) });
                System.Web.HttpContext.Current.Session["WMSData"] = null;
                throw new ClientException("Zły login/hasło lub sesja wygasła.");
            }
        }

        public ActionResult Execute<T>(Func<T> action, string viewName = null,
            string masterName = null, object options = null, Func<Exception, ErrorMessage> errorMessage = null)
        {
            try
            {
                if (viewName == null && masterName == null)
                    return View(action());
                else if (masterName == null)
                {
                    action();
                    return RedirectToAction(viewName);
                }
                else if (options == null)
                {
                    action();
                    return RedirectToAction(viewName, masterName);
                }
                else
                {
                    action();
                    return RedirectToAction(viewName, masterName, options);
                }
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
            //return new ErrorMessage("Błąd", e.ToString());
            if (e.GetType() == typeof(FaultException<ServiceException>))
                return new ErrorMessage("Błąd", (e as FaultException<ServiceException>).Detail.Message);
            else if (e.GetType() == typeof(FaultException))
                return new ErrorMessage("Błąd", (e as FaultException).Message);
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