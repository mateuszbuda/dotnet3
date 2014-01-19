using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using WMS.WebClient.Models;
using WMS.WebClient.Misc;
using WMS.ServicesInterface.DataContracts;
using WMS.ServicesInterface.DTOs;

namespace WMS.WebClient.Controllers
{
    [Authorize]
    //[InitializeSimpleMembership]
    public class AccountController : WCFProvider
    {
        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var x = AuthenticationService.AuthenticateWithToken(
                    new ServicesInterface.DataContracts.Request<ServicesInterface.DTOs.UserDto>(
                        new ServicesInterface.DTOs.UserDto() { Username = model.UserName, Password = model.Password, Remember = model.RememberMe }));

                if (x.Data != null)
                {
                    //Response.Cookies.Add(x.Data.Token);
                    HttpCookie permCookie = new HttpCookie("WMSPermissions", x.Data.Permissions.ToString());
                    permCookie.Expires = DateTime.Now.AddYears(1);
                    //permCookie.HttpOnly =;
                    Response.Cookies.Add(permCookie);
                    Session["WMSData"] = model.Password;
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);

                    return RedirectToLocal(returnUrl);
                }
            }

            ModelState.AddModelError("", "Zły login lub hasło!");
            return View(model);
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            if (Request.Cookies["WMSSession"] != null)
            {
                Response.Cookies.Add(new HttpCookie("WMSSession") { Expires = DateTime.Now.AddDays(-1) });
                Response.Cookies.Add(new HttpCookie("WMSPermissions") { Expires = DateTime.Now.AddDays(-1) });
                Session["WMSData"] = null;
            }

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Manage

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Twoje hasło zostało zmienione."
                : "";
            ViewBag.HasLocalPassword = true;
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            ViewBag.ReturnUrl = Url.Action("Manage");

            if (ModelState.IsValid && model.NewPassword == model.ConfirmPassword)
            {
                // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                bool changePasswordSucceeded = false;
                try
                {
                    UserDto u = AuthenticationService.ChangePassword(new Request<UserDto>(
                        new UserDto()
                        {
                            Username = User.Identity.Name,
                            Password = model.OldPassword,
                            NewPassword = model.NewPassword,
                            Remember = false
                        })).Data;

                    if (u != null)
                    {
                        changePasswordSucceeded = true;
                        //Response.Cookies.Add(u.Token);
                        HttpCookie permCookie = new HttpCookie("WMSPermissions", u.Permissions.ToString());
                        permCookie.Expires = DateTime.Now.AddYears(1);
                        //permCookie.HttpOnly = u.Token.HttpOnly;
                        Response.Cookies.Add(permCookie);
                        Session["WMSData"] = model.NewPassword;
                    }
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                }
                else
                {
                    ModelState.AddModelError("", "Błąd podczas zmiany hasła.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        #endregion
    }
}
