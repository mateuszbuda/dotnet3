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
    public class WarehousesController : WCFProvider
    {
        //
        // GET: /Warehouses/

        public ActionResult Index()
        {
            return View(WarehousesService.GetWarehouses(new Request()).Data);
        }

        public ActionResult Warehouse(int id = 0)
        {
            WarehouseModel w = new WarehouseModel()
            {
                Warehouse = WarehousesService.GetWarehouse(new Request<int>(id)).Data,
                Sectors = WarehousesService.GetSectors(new Request<int>(id)).Data
            };

            // Zmienić!!!
            if (w.Warehouse == null)
                return HttpNotFound();

            return View(w);
        }
    }
}
