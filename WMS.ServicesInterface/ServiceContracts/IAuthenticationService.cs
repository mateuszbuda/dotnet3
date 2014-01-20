using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading.Tasks;
using WMS.ServicesInterface.DataContracts;
using WMS.ServicesInterface.DTOs;

namespace WMS.ServicesInterface.ServiceContracts
{
    /// <summary>
    /// Interfejs do uwierzytelniania użytkowników przy logowaniu
    /// </summary>
    [ServiceContract]
    public interface IAuthenticationService
    {
        /// <summary>
        /// Uwierzytelnia zadanego użytkownika lub rzuca wyjątek w razie niepowodzenia
        /// </summary>
        /// <param name="user">Zapytanie z uwierzytelnianym użytkownikiem</param>
        /// <returns>Uwierzytnienionego użytkownika wraz z jego uprawnieniami</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<UserDto> Authenticate(Request<UserDto> user);

        /// <summary>
        /// Uwierzytelnia zadanego użytkownika za pomocą cookie lub rzuca wyjątek w razie niepowodzenia
        /// </summary>
        /// <param name="user">Zapytanie z uwierzytelnianym użytkownikiem</param>
        /// <returns>Uwierzytnienionego użytkownika wraz z jego uprawnieniami</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<UserDto> AuthenticateWithToken(Request<UserDto> user);

        /// <summary>
        /// Zmienia hasło podanego użytkownika, służy do zmiany hasła przez użytkownika.
        /// </summary>
        /// <param name="user">Zapytanie z kontem użytkownika z ustawionym nowym hasłem</param>
        /// <returns>Odpowiedź z uzytkownikiem z ustawionym nowym hasłem</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<UserDto> ChangePassword(Request<UserDto> user);
    }
}
