using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.ServicesInterface.DataContracts;
using WMS.ServicesInterface.DTOs;
using WMS.WebClient.Misc;

namespace WMS.WebClient.Controllers
{
    public class SectorsController : WCFProvider
    {
        //
        // GET: /Sectors/

        //public ActionResult Index()
        //{
        //    return View();
        //}

        //[Authoriza]
        public ActionResult GetOptions(int id)
        {
            List<SectorDto> sectors = null;

            try
            {
                sectors = WarehousesService.GetSectors(new Request<int>(id)).Data;
                ViewBag.Internal = WarehousesService.GetWarehouse(new Request<int>(id)).Data.Internal;
            }
            catch 
            {
                sectors = new List<SectorDto>();
            }

            return PartialView(sectors);
        }
    }
}
