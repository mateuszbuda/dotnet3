using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WMS.ServicesInterface.DTOs;

namespace WMS.WebClient.Models
{
    /// <summary>
    /// Model dla magazynu wraz z sektorami
    /// </summary>
    public class WarehouseModel
    {
        /// <summary>
        /// Magazyn (informacje o nim)
        /// </summary>
        public WarehouseInfoDto Warehouse { get; set; }
        /// <summary>
        /// Sektory w magazynie
        /// </summary>
        public List<SectorDto> Sectors { get; set; }
    }
}