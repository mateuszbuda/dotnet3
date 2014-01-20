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
    /// Interfejs do obsługi kont przez administratora
    /// </summary>
    [ServiceContract]
    public interface IAdministrationService
    {
        /// <summary>
        /// Pobiera konta użytkowników w systemie z danymi
        /// </summary>
        /// <param name="request">Puste zapytanie</param>
        /// <returns>Lista użytkowników</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<List<UserDto>> GetUsers(Request request);

        /// <summary>
        /// Pobiera konto konkretnego uzytkownia z danymi
        /// </summary>
        /// <param name="userId">Zapytanie z id użytkownika</param>
        /// <returns>Dane konta użytkownika z zadanym id</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<UserDto> GetUser(Request<int> userId);

        /// <summary>
        /// Dodaje nowego użytkownika
        /// </summary>
        /// <param name="user">Zapytanie z nowym kontem użytkownika, który ma zostatać dodany</param>
        /// <returns>Odpowiedź z dodanym kontem użytkownika</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<UserDto> AddNew(Request<UserDto> user);

        /// <summary>
        /// Edytuje istniejące w systemie konto użytkownika
        /// </summary>
        /// <param name="user">Zapytanie z kontem użytkownika, który ma zostatać wyedytowany</param>
        /// <returns>Odpowiedź z wyedytwanym kontem użytkownika</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<UserDto> Edit(Request<UserDto> user);

        /// <summary>
        /// Usuwa konto użytkownika, jeśli nie jest to konto administratora
        /// </summary>
        /// <param name="userId">Zapytanie z id konta użytkownika, który ma zostatać usunięty</param>
        /// <returns>Odpowiedź z informacją, czy operacja usunięcia się powiodła</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<bool> Delete(Request<int> userId);

        /// <summary>
        /// Zmienia (resetuje) hasło dla podanego użytkownika przez administratora.
        /// </summary>
        /// <param name="user">Zapytanie z kontem użytkownika z ustawionym nowym hasłem</param>
        /// <returns>Odpowiedź z uzytkownikiem z ustawionym nowym hasłem</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<UserDto> ChangePassword(Request<UserDto> user);
    }
}
