using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.ServicesInterface.DataContracts;
using WMS.ServicesInterface.DTOs;
using WMS.WebClient.Misc;
using WMS.WebClient.Models;

namespace WMS.WebClient.Controllers
{
    /// <summary>
    /// Dostarcza widoki dla stron podmenu Produkty
    /// </summary>
    public class ProductsController : WCFProvider
    {
        //
        // GET: /Products/
        /// <summary>
        /// Podmenu Produkty
        /// </summary>
        [Authorize]
        public ActionResult Index()
        {
            return Execute(() => ProductsService.GetProducts(new Request()).Data);
        }

        /// <summary>
        /// Informacja o produkcie
        /// </summary>
        /// <param name="id">Id produktu</param>
        [Authorize]
        public ActionResult Product(int id = 0)
        {
            return Execute(() =>
            {
                ProductDto p = ProductsService.GetProduct(new Request<int>(id)).Data;

                if (p == null)
                    throw new ClientException("Produkt o takim ID nie istnieje!");

                return p;
            });
        }

        /// <summary>
        /// Edycja produktu
        /// </summary>
        /// <param name="id">Id produktu</param>
        [Authorize]
        public ActionResult Edit(int id)
        {
            return Execute(() =>
            {
                return ProductsService.GetProduct(new Request<int>(id)).Data;
            });
        }

        /// <summary>
        /// Edycja produktu
        /// </summary>
        /// <param name="product">Wyedytowany produkt</param>
        /// <param name="id">Id produktu</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductDto product, int id)
        {
            return Execute(() =>
                {
                    TempData["StatusMessage"] = "Produkt '" + product.Name + "' został zapisany pomyślnie.";
                    return ProductsService.Edit(new Request<ProductDto>(product)).Data;
                }, "Index");
        }

        /// <summary>
        /// Tworzenie nowego produktu
        /// </summary>
        [Authorize]
        public ActionResult New()
        {
            return View("Edit");
        }

        /// <summary>
        /// Tworzenie nowego produktu
        /// </summary>
        /// <param name="product">Utworzony produkt</param>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(ProductDto product)
        {
            return Execute(() =>
            {
                TempData["StatusMessage"] = "Produkt '" + product.Name + "' został dodany pomyślnie.";
                return ProductsService.AddNew(new Request<ProductDto>(product));
            }, "Index");
        }
    }
}
