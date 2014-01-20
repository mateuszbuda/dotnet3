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

        [Authorize]
        public ActionResult Edit(int id = -1)
        {
            return Execute(() =>
                {
                    if (id == -1)
                        throw new ClientException("Taki sektor nie istnieje.");

                    return WarehousesService.GetSector(new Request<int>(id)).Data;
                });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SectorDto sector, int id = -1)
        {
            SectorDto s;
            try
            {
                s = WarehousesService.GetSector(new Request<int>(id)).Data;
            }
            catch(Exception e)
            {
                return View("DataError", GetErrorMessage(e));
            }

            return Execute(() =>
                {
                    if (id == -1)
                        throw new ClientException("Taki sektor nie istnieje.");

                    //s = WarehousesService.GetSector(new Request<int>(id)).Data;

                    s.Limit = sector.Limit;

                    return WarehousesService.EditSector(new Request<SectorDto>(s)).Data;
                }, "Warehouse", "Warehouses", new { id = s.WarehouseId });
        }

        [Authorize]
        public ActionResult New(int id = -1)
        {
            ViewBag.WarehouseId = id;

            return View("Edit");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(SectorDto sector, int id = -1)
        {
            return Execute(() =>
                {
                    sector.WarehouseId = id;
                    sector.Number = WarehousesService.GetNextSectorNumber(new Request<int>(id)).Data;
                    sector.Deleted = false;

                    return WarehousesService.AddSector(new Request<SectorDto>(sector)).Data;
                }, "Warehouse", "Warehouses", new { id = id });
        }

        [Authorize]
        public ActionResult Delete(int id = -1)
        {
            SectorDto s;

            try
            {
                s = WarehousesService.GetSector(new Request<int>(id)).Data;
            }
            catch (Exception e)
            {
                return View("DataError", GetErrorMessage(e));
            }

            return Execute(() =>
                {
                    bool b = WarehousesService.DeleteSectorIfEmpty(new Request<int>(id)).Data;

                    if (!b)
                        throw new ClientException("Sektor nie jest pusty.");

                    return true;
                }, "Warehouse", "Warehouses", new { id = s.WarehouseId });
        }
    }
}
