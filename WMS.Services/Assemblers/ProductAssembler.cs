using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using WMS.DatabaseAccess.Entities;
using WMS.ServicesInterface.DataContracts;
using WMS.ServicesInterface.DTOs;

namespace WMS.Services.Assemblers
{
    /// <summary>
    /// Klasa odpowiada za konwersję pomiędzy obiektami związanymi z produktem
    /// mapowanymi do bazy a obiektami-paczkami przesyłanymi do i z serwera.
    /// </summary>
    public class ProductAssembler
    {
        /// <summary>
        /// Konwersja z bazodanowego produktu na produkt-paczkę do komunikacji z serwerem.
        /// </summary>
        /// <param name="product">Konwertowany produkt</param>
        /// <returns>Przekonwertowany produkt</returns>
        public ProductDto ToDto(Product product)
        {
            return new ProductDto()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                ProductionDate = product.Date,
                Version = product.Version
            };
        }

        /// <summary>
        /// Konwersja z bazodanowego produktu na produkt-paczkę do komunikacji z serwerem.
        /// Zawiera informacje o ilości produktów i jest wkorzysywana w partii.
        /// </summary>
        /// <param name="groupDetails">Konwertowane obiekt informacji o produktach w partii</param>
        /// <returns>PRzekonwertowany obiekt</returns>
        public ProductDetailsDto ToDetailsDto(GroupDetails groupDetails)
        {
            return new ProductDetailsDto()
            {
                Id = groupDetails.Product.Id,
                Name = groupDetails.Product.Name,
                Price = groupDetails.Product.Price,
                ProductionDate = groupDetails.Product.Date,
                Count = groupDetails.Count
            };
        }

        /// <summary>
        /// Konwersja z produktu-paczki do komunikacji z serwerem na produkt bazodanowy.
        /// </summary>
        /// <param name="produkt">Konwertowany produkt</param>
        /// <param name="ent">Edytowany bazodanowy produkt</param>
        /// <returns>Przekonwertowany produkt</returns>
        public Product ToEntity(ProductDto produkt, Product ent = null)
        {
            if (ent != null && !produkt.Version.SequenceEqual(ent.Version))
                throw new FaultException<ServiceException>(new ServiceException("Ktoś przed chwilą zmodyfikował dane.\nSpróbuj jeszcze raz."));

            ent = ent ?? new Product();

            ent.Name = produkt.Name;
            ent.Date = produkt.ProductionDate;
            ent.Price = produkt.Price;

            return ent;
        }
    }
}