using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WMS.ServicesInterface;
using WMS.ServicesInterface.ServiceContracts;
using WMS.ServicesInterface.DataContracts;
using WMS.ServicesInterface.DTOs;
using WMS.Services.Assemblers;
using WMS.DatabaseAccess.Entities;

namespace WMS.Services
{
    /// <summary>
    /// Serwis służącydo uwierzytelniania uzytkowników
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerSession, IncludeExceptionDetailInFaults = true)]
    public class AuthenticationService : ServiceBase, IAuthenticationService
    {
        /// <summary>
        /// Uwierzytelnia zadanego użytkownika lub rzuca wyjątek w razie niepowodzenia
        /// </summary>
        /// <param name="user">Zapytanie z uwierzytelnianym użytkownikiem</param>
        /// <returns>Uwierzytnienionego użytkownika wraz z jego uprawnieniami</returns>
        public Response<UserDto> Authenticate(Request<UserDto> user)
        {
            var u = userAssembler.ToEntity(user.Content);
            User ret = null;
            Transaction(tc => ret = tc.Entities.Users.Where(x => x.Username == u.Username).FirstOrDefault());

            if (ret == null || ret.Password != u.Password)
                throw new FaultException<ServiceException>(new ServiceException("Zły login lub hasło!"));

            return new Response<UserDto>(user.Id, userAssembler.ToDto(ret));
        }

        public Response<List<UserDto>> GetUsers(Request request)
        {
            CheckPermissions(PermissionLevel.Administrator);
            return new Response<List<UserDto>>(request.Id, Transaction(tc =>
                tc.Entities.Users.Select(userAssembler.ToDto).ToList()));
        }

        public Response<UserDto> GetUser(Request<int> userId)
        {
            CheckPermissions(PermissionLevel.Administrator);
            return new Response<UserDto>(userId.Id, Transaction(tc =>
                tc.Entities.Users.Where(x => x.Id == userId.Content).
                Select(userAssembler.ToDto).FirstOrDefault()));
        }

        public Response<UserDto> AddNew(Request<UserDto> user)
        {
            CheckPermissions(PermissionLevel.Administrator);
            User u = null;
            Transaction(tc => u = tc.Entities.Users.Add(userAssembler.ToEntity(user.Content)));
            return new Response<UserDto>(user.Id, userAssembler.ToDto(u));
        }

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


        public Response<bool> Delete(Request<int> userId)
        {
            CheckPermissions(PermissionLevel.Administrator);
            bool ret = false;

            Transaction(tc =>
            {
                User user = tc.Entities.Users.Find(userId.Content);
                if (user == null)
                    throw new FaultException<ServiceException>(new ServiceException("Nie ma takiego użytkownika."));
                tc.Entities.Users.Remove(user);
            });

            return new Response<bool>(userId.Id, ret);
        }
    }
}
