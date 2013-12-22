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
    /// Interfejs do wymiany informacji o produktach
    /// </summary>
    [ServiceContract]
    public interface IProductsService
    {
        /// <summary>
        /// Pobiera informacje o Wszystkich produktach wprowadzonych do systemu
        /// </summary>
        /// <param name="request">Puste zapytanie</param>
        /// <returns>Odpowiedź z listą produktów</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<List<ProductDto>> GetProducts(Request request);

        /// <summary>
        /// Pobiera informacje o konkretnym produkcie
        /// </summary>
        /// <param name="productId">Zapytanie z id produktu</param>
        /// <returns>Odpowiedź z informacjami o produkcie</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<ProductDto> GetProduct(Request<int> productId);

        /// <summary>
        /// Dodaje nowy produkt
        /// </summary>
        /// <param name="product">Zapytanie z produktem do dodania</param>
        /// <returns>Odpowiedź z dodanym produktem</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<bool> AddNew(Request<ProductDto> product);

        /// <summary>
        /// Edytuje istniejący produkt
        /// </summary>
        /// <param name="product">Zapytanie z produktem do wyedytowania</param>
        /// <returns>Odpowiedź z wyedytowanym produktem</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<bool> Edit(Request<ProductDto> product);
    }
}
