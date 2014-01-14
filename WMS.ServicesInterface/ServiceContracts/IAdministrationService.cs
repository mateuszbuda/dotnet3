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
    /// Interfejs administracyjny
    /// </summary>
    [ServiceContract]
    public interface IAdministrationService
    {
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

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<UserDto> ChangePassword(Request<UserDto> user);
    }
}
