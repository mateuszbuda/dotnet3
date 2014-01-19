using System;
using System.Linq;
using System.ServiceModel;
using WMS.ServicesInterface.ServiceContracts;
using WMS.ServicesInterface.DataContracts;
using WMS.ServicesInterface.DTOs;
using WMS.DatabaseAccess.Entities;
using System.Web.Security;
using System.ServiceModel.Activation;

namespace WMS.Services
{
    /// <summary>
    /// Serwis służącydo uwierzytelniania uzytkowników
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerSession, IncludeExceptionDetailInFaults = true)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
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

        public Response<UserDto> AuthenticateWithToken(Request<UserDto> user)
        {
            var u = userAssembler.ToEntity(user.Content);
            User ret = null;
            Transaction(tc => ret = tc.Entities.Users.Where(x => x.Username == u.Username).FirstOrDefault());

            UserDto userDto = null;

            if (ret != null && ret.Password == u.Password)
            {
                userDto = userAssembler.ToDto(ret);
                //userDto.Token = FormsAuthentication.GetAuthCookie(userDto.Username, user.Content.Remember);
            }

            return new Response<UserDto>(user.Id, userDto);
        }

        public Response<UserDto> ChangePassword(Request<UserDto> user)
        {
            var u = userAssembler.ToEntity(user.Content);
            User us = null;
            Transaction(tc => us = tc.Entities.Users.Where(x => x.Username == u.Username).FirstOrDefault());

            UserDto ret = null;

            if (us != null && us.Password == u.Password)
            {
                UserDto newUser = new UserDto()
                {
                    Username = user.Content.Username,
                    Password = user.Content.NewPassword,
                    PermissionsVal = u.Permissions
                };

                Transaction(tc =>
                    {
                        us = tc.Entities.Users.Where(x => x.Username == u.Username).FirstOrDefault();
                        us.Password = userAssembler.ToEntity(newUser).Password;
                    });

                ret = userAssembler.ToDto(us);
                ret.Token = FormsAuthentication.GetAuthCookie(ret.Username, false);
            }

            return new Response<UserDto>(user.Id, ret);
        }
    }
}
