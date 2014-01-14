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
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerSession, IncludeExceptionDetailInFaults = true)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class AdministrationService : ServiceBase, IAdministrationService
    {

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

        public Response<UserDto> AddNew(Request<UserDto> user)
        {
            CheckPermissions(PermissionLevel.Administrator);
            User u = null;
            Transaction(tc =>
                {
                    var us = tc.Entities.Users.Where(x => x.Username == user.Content.Username).FirstOrDefault();
                    if(us != null)
                        throw new FaultException<ServiceException>(new ServiceException("Użytkownik o takiej nazwie już istnieje."));

                    u = tc.Entities.Users.Add(userAssembler.ToEntity(user.Content));
                });
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
