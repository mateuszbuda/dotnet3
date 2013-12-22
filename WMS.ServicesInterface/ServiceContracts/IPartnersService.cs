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
    /// Interfejs do wymiany informacji o partnerach
    /// </summary>
    [ServiceContract]
    public interface IPartnersService
    {
        /// <summary>
        /// Pobiera informacje o wszyskich partnerach z systemu, co ciekawe potencjalnie tych usuniętych też
        /// </summary>
        /// <param name="request">Puste zapytanie</param>
        /// <returns>Odpowiedź z listą partnerów</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<List<PartnerSimpleDto>> GetPartners(Request request);

        /// <summary>
        /// Pobiera dane o partnerze o zadanym id, nawet jeśli został usunięty
        /// </summary>
        /// <param name="partnerId">Zapytanie z id partnera</param>
        /// <returns>Odpowiedź z danymi o partnerze z zadanym id</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<PartnerDto> GetPartner(Request<int> PartnerId);

        /// <summary>
        /// Pobiera listę przesunięć, których nadawcą lub odbiorcą był dany partner
        /// </summary>
        /// <param name="partnerId">Zapytanie z id partnera</param>
        /// <returns>Odpowiedź z listą żądanych przesunięć</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<List<ShiftHistoryDto>> GetPartnerHistory(Request<int> PartnerId);

        /// <summary>
        /// Pobiera dane o partnerze z magazynem o zadanym id, nawet jeśli ten partner został usunięty
        /// </summary>
        /// <param name="warehouseId">Zapytanie z id magazynu partnera</param>
        /// <returns>Odpowiedź z danymi o partnerze z zadanym id</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<PartnerDto> GetPartnerByWarehouse(Request<int> warehouseId);

        /// <summary>
        /// Dodaje nowego partnera, lub rzuca wjątek w przypadku niepowodzenia
        /// </summary>
        /// <param name="partner">Zapytanie z partnerem do dodania</param>
        /// <returns>Odpowiedź z dodanym partnerem</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<PartnerDto> AddNew(Request<PartnerDto> partner);

        /// <summary>
        /// Etytuje dane o istniejącym partnerze
        /// </summary>
        /// <param name="partner">Zapytanie z wyedytowanym partnerem</param>
        /// <returns>Odpowiedź z wyedytowanym partnerem</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<PartnerDto> Update(Request<PartnerDto> partner);
    }
}
