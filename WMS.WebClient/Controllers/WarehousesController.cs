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
    /// Dostarcza widoki dla stron podmenu Magazyny
    /// </summary>
    [RequireHttps]
    public class WarehousesController : WCFProvider
    {
        //
        // GET: /Warehouses/
        /// <summary>
        /// Podmenu Magazyny
        /// </summary>
        [Authorize]
        public ActionResult Index()
        {
            return Execute(() => WarehousesService.GetWarehouses(new Request()).Data);
        }

        /// <summary>
        /// Podgląd magazynu (jego sektorów)
        /// </summary>
        /// <param name="id">Id magazynu</param>
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

        /// <summary>
        /// Ifnormacja o magazynie (dane kontaktowe)
        /// </summary>
        /// <param name="id">Id magazynu</param>
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

        /// <summary>
        /// Edycja informacji o magazynie
        /// </summary>
        /// <param name="id">Id magazynu</param>
        [Authorize]
        public ActionResult Edit(int id)
        {
            return Execute(() =>
                {
                    return WarehousesService.GetWarehouse(new Request<int>(id)).Data;
                });
        }

        /// <summary>
        /// Edycja informacji o magazynie
        /// </summary>
        /// <param name="warehouse">Wyedytowany magazyn</param>
        /// <param name="id">Id magazynu</param>
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

        /// <summary>
        /// Tworzenie nowego magazynu
        /// </summary>
        [Authorize]
        public ActionResult New()
        {
            return View("Edit");
        }

        /// <summary>
        /// Tworzenie nowego magazynu
        /// </summary>
        /// <param name="warehouse">Utworzony magazyn</param>
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

        /// <summary>
        /// Usuwanie magazynu
        /// </summary>
        /// <param name="id">Id magazynu</param>
        /// <returns>Informacja czy operacja siępowiodła</returns>
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

        /// <summary>
        /// Podgląd sektora (grup, które w nim się znajdują)
        /// </summary>
        /// <param name="id">Id sektora</param>
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
