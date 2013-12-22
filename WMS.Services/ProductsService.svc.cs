using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WMS.Services.Assemblers;
using WMS.ServicesInterface;
using WMS.ServicesInterface.DataContracts;
using WMS.ServicesInterface.DTOs;
using WMS.ServicesInterface.ServiceContracts;

namespace WMS.Services
{
    /// <summary>
    /// Serwis obsługujący zapytania związane z produktami
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerSession, IncludeExceptionDetailInFaults = true)]
    public class ProductsService : ServiceBase, IProductsService
    {
        /// <summary>
        /// Pobiera informacje o Wszystkich produktach wprowadzonych do systemu
        /// </summary>
        /// <param name="request">Puste zapytanie</param>
        /// <returns>Odpowiedź z listą produktów</returns>
        public Response<List<ProductDto>> GetProducts(Request request)
        {
            CheckPermissions(PermissionLevel.User);
            return new Response<List<ProductDto>>(request.Id, Transaction(tc =>
                tc.Entities.Products.Select(productAssembler.ToDto).ToList()));
        }

        /// <summary>
        /// Pobiera informacje o konkretnym produkcie
        /// </summary>
        /// <param name="productId">Zapytanie z id produktu</param>
        /// <returns>Odpowiedź z informacjami o produkcie</returns>
        public Response<ProductDto> GetProduct(Request<int> productId)
        {
            CheckPermissions(PermissionLevel.User);
            return new Response<ProductDto>(productId.Id, Transaction(tc =>
                productAssembler.ToDto(tc.Entities.Products.Find(productId.Content))));
        }

        /// <summary>
        /// Dodaje nowy produkt
        /// </summary>
        /// <param name="product">Zapytanie z produktem do dodania</param>
        /// <returns>Odpowiedź z dodanym produktem</returns>
        public Response<bool> AddNew(Request<ProductDto> product)
        {
            CheckPermissions(PermissionLevel.Manager);
            Transaction(tc => tc.Entities.Products.Add(productAssembler.ToEntity(product.Content)));
            return new Response<bool>(product.Id, true);
        }

        /// <summary>
        /// Edytuje istniejący produkt
        /// </summary>
        /// <param name="product">Zapytanie z produktem do wyedytowania</param>
        /// <returns>Odpowiedź z wyedytowanym produktem</returns>
        public Response<bool> Edit(Request<ProductDto> product)
        {
            CheckPermissions(PermissionLevel.Manager);
            Transaction(tc =>
                {
                    var p = tc.Entities.Products.Find(product.Content.Id);
                    if (p == null)
                        throw new FaultException<ServiceException>(new ServiceException("Ten produkt nie istnieje!"));

                    productAssembler.ToEntity(product.Content, p);
                });
            return new Response<bool>(product.Id, true);
        }
    }
}
