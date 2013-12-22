using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WMS.ServicesInterface.DTOs;

namespace WMS.WebClient.Models
{
    public class WarehouseModel
    {
        public WarehouseInfoDto Warehouse { get; set; }
        public List<SectorDto> Sectors { get; set; }
    }
}