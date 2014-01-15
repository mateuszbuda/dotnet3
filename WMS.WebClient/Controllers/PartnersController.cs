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
    public class PartnersController : WCFProvider
    {
        //
        // GET: /Partners/

        //[Authorize]
        public ActionResult Index()
        {
            return Execute(() => PartnersService.GetPartnersWithWarehouses(new Request()).Data);
        }

        //[Authorize]
        public ActionResult Partner(int id = 0)
        {
            return Execute(() =>
            {
                PartnerDto p = PartnersService.GetPartner(new Request<int>(id)).Data;

                if (p == null)
                    throw new ClientException("Partner o takim ID nie istnieje!");

                return p;
            });
        }

        //[Authorize]
        public ActionResult Edit(int id)
        {
            return Execute(() =>
            {
                return PartnersService.GetPartner(new Request<int>(id)).Data;
            });
        }

        //[Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PartnerDto partner, int id)
        {
            return Execute(() =>
            {
                TempData["StatusMessage"] = "Partner '" + partner.Name + "' został zapisany pomyślnie.";
                return PartnersService.UpdateWithoutWarehouse(new Request<PartnerDto>(partner)).Data;
            }, "Index");
        }

        //[Authorize]
        public ActionResult New()
        {
            return View("Edit");
        }

        //[Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(PartnerDto partner)
        {
            return Execute(() =>
            {
                TempData["StatusMessage"] = "Partner '" + partner.Name + "' został dodany pomyślnie.";
                return PartnersService.AddNew(new Request<PartnerDto>(partner));
            }, "Index");
        }

        //[Authorize]
        public ActionResult Warehouse(int id = 0)
        {
            return Execute(() =>
            {
                WarehouseInfoDto ws = WarehousesService.GetWarehouse(new Request<int>(id)).Data;

                if (ws == null)
                    throw new ClientException("Magazyn o takim ID nie istnieje!");

                ViewBag.PartnerName = PartnersService.GetPartnerByWarehouse(new Request<int>(id)).Data.Name;

                return ws;
            });
        }

        //[Authorize]
        public ActionResult EditWarehouse(int id)
        {
            return Execute(() =>
            {
                ViewBag.PartnerName = PartnersService.GetPartnerByWarehouse(new Request<int>(id)).Data.Name;

                return WarehousesService.GetWarehouse(new Request<int>(id)).Data;
            });
        }

        //[Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditWarehouse(WarehouseInfoDto warehouse, int id)
        {
            return Execute(() =>
            {
                TempData["StatusMessage"] = "Magazyn '" + warehouse.Name + "' został zapisany pomyślnie.";
                return WarehousesService.EditParnersWarehouse(new Request<WarehouseInfoDto>(warehouse)).Data;
            }, "Index");
        }
    }
}
