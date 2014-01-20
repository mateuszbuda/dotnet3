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
    [RequireHttps]
    public class WarehousesController : WCFProvider
    {
        //
        // GET: /Warehouses/

        [Authorize]
        public ActionResult Index()
        {
            return Execute(() => WarehousesService.GetWarehouses(new Request()).Data);
        }

        [Authorize]
        public ActionResult Warehouse(int id = 0)
        {
            return Execute(() =>
                {
                    WarehouseModel w = new WarehouseModel()
                    {
                        Warehouse = WarehousesService.GetWarehouse(new Request<int>(id)).Data,
                        Sectors = WarehousesService.GetSectors(new Request<int>(id)).Data
                    };

                    if (w.Warehouse == null)
                        throw new ClientException("Magazyn o takim ID nie istnieje!");

                    return w;
                });
        }

        [Authorize]
        public ActionResult Show(int id)
        {
            try
            {
                var w = WarehousesService.GetWarehouse(new Request<int>(id)).Data;
                if (w == null)
                    throw new ClientException("Magazyn o takim ID nie istnieje!");

                if (w.Internal)
                    return RedirectToAction("Warehouse", "Warehouses", new { id = w.Id });
                else
                {
                    var p = PartnersService.GetPartnerByWarehouse(new Request<int>(id)).Data;
                    return RedirectToAction("Partner", "Partners", new { id = p.Id });
                }
            }
            catch (Exception e)
            {
                return View("DataError", GetErrorMessage(e));
            }
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            return Execute(() =>
                {
                    return WarehousesService.GetWarehouse(new Request<int>(id)).Data;
                });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(WarehouseInfoDto warehouse, int id)
        {
            return Execute(() =>
                {
                    TempData["StatusMessage"] = "Magazyn '" + warehouse.Name + "' został zapisany pomyślnie.";
                    return WarehousesService.Edit(new Request<WarehouseInfoDto>(warehouse)).Data;
                }, "Index");
        }

        [Authorize]
        public ActionResult New()
        {
            return View("Edit");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(WarehouseInfoDto warehouse)
        {
            return Execute(() =>
                {
                    TempData["StatusMessage"] = "Magazyn '" + warehouse.Name + "' został dodany pomyślnie.";
                    return WarehousesService.AddNew(new Request<WarehouseInfoDto>(warehouse));
                }, "Index");
        }

        [Authorize]
        public ActionResult Delete(int id = -1)
        {
            return Execute(() =>
                {
                    if (id == -1)
                        throw new ClientException("Złe warehouseId magazynu!");

                    string name = WarehousesService.GetWarehouse(new Request<int>(id)).Data.Name;

                    bool del = WarehousesService.DeleteIfEmpty(new Request<int>(id)).Data;

                    if (!del)
                        throw new ClientException("Magazyn '" + name + "' nie jest pusty!");

                    TempData["StatusMessage"] = "Pomyślnie usunięto magazyn '" + name + "'.";

                    return true;
                }, "Index");
        }

        [Authorize]
        public ActionResult Sector(int id = -1)
        {
            return Execute(() =>
                {
                    if (id == -1)
                        throw new ClientException("Taki sektor nie istnieje.");

                    ViewBag.SectorInfo = WarehousesService.GetSector(new Request<int>(id)).Data;

                    return GroupsService.GetSectorGroups(new Request<int>(id)).Data;
                });
        }
    }
}
