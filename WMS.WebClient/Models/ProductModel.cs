using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WMS.ServicesInterface.DTOs;

namespace WMS.WebClient.Models
{
    /// <summary>
    /// Model dla produktu
    /// </summary>
    public class ProductModel
    {
        /// <summary>
        /// Produkt
        /// </summary>
        public ProductDto Product { get; set; }
    }
}