﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.ServicesInterface;
using WMS.ServicesInterface.DataContracts;
using WMS.ServicesInterface.DTOs;
using WMS.WebClient.Misc;

namespace WMS.WebClient.Controllers
{
    /// <summary>
    /// Dostarcza widoki dla stron podmenu Panelu Administracyjnego
    /// </summary>
    public class AdminController : WCFProvider
    {
        //
        // GET: /Admin/
        /// <summary>
        /// Panel Administratora
        /// </summary>
        [Authorize]
        public ActionResult Index()
        {
            return Execute(() =>
                {
                    if (ViewBag.Permissions > PermissionLevel.Administrator)
                        throw new ClientException("Brak uprawnień.");

                    return AdministrationService.GetUsers(new Request()).Data;
                });
        }

        /// <summary>
        /// Edycja konta uzytkownika 
        /// </summary>
        /// <param name="id">Id edytowanego użytkownika</param>
        [Authorize]
        public ActionResult Edit(int id = -1)
        {
            return Execute(() =>
                {
                    if (ViewBag.Permissions > PermissionLevel.Administrator)
                        throw new ClientException("Brak uprawnień.");

                    if (id == -1)
                        throw new ClientException("Użytkownik o takim ID nie istnieje.");

                    return AdministrationService.GetUser(new Request<int>(id)).Data;
                });
        }

        /// <summary>
        /// Edycja konta uzytkownika
        /// </summary>
        /// <param name="permissions">Nadane uprawnienia</param>
        /// <param name="id">Id wyedytowanego użytkownika</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int permissions, int id)
        {
            return Execute(() =>
                {
                    if (ViewBag.Permissions > PermissionLevel.Administrator)
                        throw new ClientException("Brak uprawnień.");

                    if (permissions > 3 || permissions < 0)
                        throw new ClientException("Złe uprawnienia.");

                    var u = AdministrationService.GetUser(new Request<int>(id)).Data;
                    u.Permissions = (PermissionLevel)permissions;
                    u.PermissionsVal = (int)permissions;

                    return AdministrationService.Edit(new Request<UserDto>(u));
                }, "Index");
        }

        /// <summary>
        /// Tworzenie nowego uzytkownika
        /// </summary>
        [Authorize]
        public ActionResult NewUser()
        {
            return Execute(() =>
                {
                    if (ViewBag.Permissions > PermissionLevel.Administrator)
                        throw new ClientException("Brak uprawnień.");

                    return default(UserDto);
                });
        }

        /// <summary>
        /// Tworzenie nowego użytkownika
        /// </summary>
        /// <param name="user">Nowo utworzony uzytkownik</param>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewUser(UserDto user)
        {
            return Execute(() =>
            {
                if (ViewBag.Permissions > PermissionLevel.Administrator)
                    throw new ClientException("Brak uprawnień.");

                if (user.Password != user.ConfirmPassword)
                    throw new ClientException("Hasła nie są identyczne.");

                if (user.PermissionsVal > 3 || user.PermissionsVal < 0)
                    throw new ClientException("Złe uprawnienia.");

                user.Permissions = (PermissionLevel)user.PermissionsVal;

                return AdministrationService.AddNew(new Request<UserDto>(user)).Data;
            }, "Index");
        }

        /// <summary>
        /// Usuwanie konta uzytkownika
        /// </summary>
        /// <param name="id">Id usuwanego konta</param>
        [Authorize]
        public ActionResult Delete(int id = -1)
        {
            return Execute(() =>
                {
                    if (ViewBag.Permissions > PermissionLevel.Administrator)
                        throw new ClientException("Brak uprawnień.");

                    if (id == -1)
                        throw new ClientException("Złe ID.");
                    return AdministrationService.Delete(new Request<int>(id)).Data;
                }, "Index");
        }

        /// <summary>
        /// Resetowanie hasła uzytkownika
        /// </summary>
        /// <param name="id">Id użytkownika</param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Reset(int id = -1)
        {
            return Execute(() =>
                {
                    if (ViewBag.Permissions > PermissionLevel.Administrator)
                        throw new ClientException("Brak uprawnień.");

                    if (id == -1)
                        throw new ClientException("Złe ID");

                    string u = AdministrationService.GetUser(new Request<int>(id)).Data.Username;

                    return new UserDto() { Username = u };
                });
        }

        /// <summary>
        /// Resetowanie hasła użytkownika
        /// </summary>
        /// <param name="user">Użytkownik, któremu zostało zresetowane hasło</param>
        /// <param name="id">Id użytkownika</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reset(UserDto user, int id = -1)
        {
            return Execute(() =>
                {
                    if (ViewBag.Permissions > PermissionLevel.Administrator)
                        throw new ClientException("Brak uprawnień.");

                    if (id == -1)
                        throw new ClientException("Złe ID");

                    if (user.Password != user.ConfirmPassword)
                        throw new ClientException("Hasła nie są identyczne.");

                    user.Id = id;

                    return AdministrationService.ChangePassword(new Request<UserDto>(user)).Data;
                }, "Index");
        }
    }
}
