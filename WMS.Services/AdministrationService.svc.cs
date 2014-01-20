using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using WMS.DatabaseAccess.Entities;
using WMS.ServicesInterface;
using WMS.ServicesInterface.DataContracts;
using WMS.ServicesInterface.DTOs;
using WMS.ServicesInterface.ServiceContracts;

namespace WMS.Services
{
    /// <summary>
    /// Serwis służący do obsługi kont przez administratora
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerSession, IncludeExceptionDetailInFaults = true)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class AdministrationService : ServiceBase, IAdministrationService
    {
        /// <summary>
        /// Pobiera konta użytkowników w systemie z danymi
        /// </summary>
        /// <param name="request">Puste zapytanie</param>
        /// <returns>Lista użytkowników</returns>
        public Response<List<UserDto>> GetUsers(Request request)
        {
            CheckPermissions(PermissionLevel.Administrator);
            return new Response<List<UserDto>>(request.Id, Transaction(tc =>
                tc.Entities.Users.Select(userAssembler.ToDto).ToList()));
        }

        /// <summary>
        /// Pobiera konto konkretnego uzytkownia z danymi
        /// </summary>
        /// <param name="userId">Zapytanie z id użytkownika</param>
        /// <returns>Dane konta użytkownika z zadanym id</returns>
        public Response<UserDto> GetUser(Request<int> userId)
        {
            CheckPermissions(PermissionLevel.Administrator);
            return new Response<UserDto>(userId.Id, Transaction(tc =>
                tc.Entities.Users.Where(x => x.Id == userId.Content).
                Select(userAssembler.ToDto).FirstOrDefault()));
        }

        /// <summary>
        /// Zmienia (resetuje) hasło dla podanego użytkownika przez administratora.
        /// </summary>
        /// <param name="user">Zapytanie z kontem użytkownika z ustawionym nowym hasłem</param>
        /// <returns>Odpowiedź z uzytkownikiem z ustawionym nowym hasłem</returns>
        public Response<UserDto> ChangePassword(Request<UserDto> user)
        {
            UserDto ret = null;

            Transaction(tc =>
                {
                    var u = tc.Entities.Users.Where(x => x.Id == user.Content.Id).FirstOrDefault();
                    u.Password = userAssembler.ToEntity(user.Content).Password;
                });

            return new Response<UserDto>(user.Id, ret);
        }

        /// <summary>
        /// Dodaje nowego użytkownika
        /// </summary>
        /// <param name="user">Zapytanie z nowym kontem użytkownika, który ma zostatać dodany</param>
        /// <returns>Odpowiedź z dodanym kontem użytkownika</returns>
        public Response<UserDto> AddNew(Request<UserDto> user)
        {
            CheckPermissions(PermissionLevel.Administrator);
            User u = null;
            Transaction(tc =>
                {
                    var us = tc.Entities.Users.Where(x => x.Username == user.Content.Username).FirstOrDefault();
                    if (us != null)
                        throw new FaultException<ServiceException>(new ServiceException("Użytkownik o takiej nazwie już istnieje."));

                    u = tc.Entities.Users.Add(userAssembler.ToEntity(user.Content));
                });
            return new Response<UserDto>(user.Id, userAssembler.ToDto(u));
        }

        /// <summary>
        /// Edytuje istniejące w systemie konto użytkownika
        /// </summary>
        /// <param name="user">Zapytanie z kontem użytkownika, który ma zostatać wyedytowany</param>
        /// <returns>Odpowiedź z wyedytwanym kontem użytkownika</returns>
        public Response<UserDto> Edit(Request<UserDto> user)
        {
            CheckPermissions(PermissionLevel.Administrator);
            User u = null;
            Transaction(tc =>
            {
                u = tc.Entities.Users.Find(user.Content.Id);
                if (u == null)
                    throw new FaultException<ServiceException>(new ServiceException("Taki użytkownik nie istnieje!"));
                string hash = u.Password;
                userAssembler.ToEntity(user.Content, u);
                u.Password = hash;
            });
            return new Response<UserDto>(user.Id, userAssembler.ToDto(u));
        }

        /// <summary>
        /// Usuwa konto użytkownika, jeśli nie jest to konto administratora
        /// </summary>
        /// <param name="userId">Zapytanie z id konta użytkownika, który ma zostatać usunięty</param>
        /// <returns>Odpowiedź z informacją, czy operacja usunięcia się powiodła</returns>
        public Response<bool> Delete(Request<int> userId)
        {
            CheckPermissions(PermissionLevel.Administrator);
            bool ret = false;

            Transaction(tc =>
            {
                User user = tc.Entities.Users.Find(userId.Content);
                if (user == null)
                    throw new FaultException<ServiceException>(new ServiceException("Nie ma takiego użytkownika."));
                // ZMIANIA! JAK COŚ NIE DAŁA TO ZAKOMENTOWAć!
                if ((PermissionLevel)user.Permissions != PermissionLevel.Administrator)
                    // KONIEC ZMIANY ;)
                    tc.Entities.Users.Remove(user);
            });

            return new Response<bool>(userId.Id, ret);
        }
    }
}
