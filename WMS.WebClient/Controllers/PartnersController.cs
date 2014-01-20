using System;
using System.Linq;
using System.Web.Mvc;
using WMS.ServicesInterface.DataContracts;
using WMS.ServicesInterface.DTOs;
using WMS.WebClient.Misc;

namespace WMS.WebClient.Controllers
{
    /// <summary>
    /// Dostarcza widoki dla stron podmenu Partnerzy
    /// </summary>
    public class PartnersController : WCFProvider
    {
        //
        // GET: /Partners/
        /// <summary>
        /// Podmenu Partnerzy
        /// </summary>
        [Authorize]
        public ActionResult Index()
        {
            return Execute(() => PartnersService.GetPartnersWithWarehouses(new Request()).Data);
        }

        /// <summary>
        /// Informacje o partnerze
        /// </summary>
        /// <param name="id">Id partnera</param>
        [Authorize]
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

        /// <summary>
        /// Edycja partnera
        /// </summary>
        /// <param name="id">Id partnera</param>
        [Authorize]
        public ActionResult Edit(int id)
        {
            return Execute(() =>
            {
                return PartnersService.GetPartner(new Request<int>(id)).Data;
            });
        }

        /// <summary>
        /// Edycja partnera
        /// </summary>
        /// <param name="partner">Wyedytowny partner</param>
        /// <param name="id">Id partnera</param>
        /// <returns></returns>
        [Authorize]
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

        /// <summary>
        /// Tworzenie nowego partnera
        /// </summary>
        [Authorize]
        public ActionResult New()
        {
            return View("Edit");
        }

        /// <summary>
        /// Tworzenie nowego partnera
        /// </summary>
        /// <param name="partner">Utworzony partner</param>
        [Authorize]
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

        /// <summary>
        /// Informacje o magazynie partnera
        /// </summary>
        /// <param name="id">Id magazynu</param>
        [Authorize]
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

        /// <summary>
        /// Edycja magazynu partnera
        /// </summary>
        /// <param name="id">Id magazynu</param>
        [Authorize]
        public ActionResult EditWarehouse(int id)
        {
            return Execute(() =>
            {
                ViewBag.PartnerName = PartnersService.GetPartnerByWarehouse(new Request<int>(id)).Data.Name;

                return WarehousesService.GetWarehouse(new Request<int>(id)).Data;
            });
        }

        /// <summary>
        /// Edycja magazynu partnera
        /// </summary>
        /// <param name="warehouse">Wyedytowany magazyn</param>
        /// <param name="id">Id magazynu</param>
        /// <returns></returns>
        [Authorize]
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
