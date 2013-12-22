using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<List<UserDto>> GetUsers(Request request);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<UserDto> GetUser(Request<int> userId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<UserDto> AddNew(Request<UserDto> user);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<UserDto> Edit(Request<UserDto> user);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<bool> Delete(Request<int> userId);
    }
}
