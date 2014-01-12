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
    public class GroupsController : WCFProvider
    {
        //
        // GET: /Groups/

        //[Authorize]
        public ActionResult Index()
        {
            return Execute(() =>
                {
                    return GroupsService.GetShifts(new Request()).Data;
                });
        }

        //[Authorize]
        public ActionResult Group(int id = -1)
        {
            return Execute(() =>
                {
                    if (id == -1)
                        throw new ClientException("Partia o takim id nie istnieje.");

                    ViewBag.GroupInfo = GroupsService.GetGroupInfo(new Request<int>(id)).Data;

                    return GroupsService.GetGroupDetails(new Request<int>(id)).Data;
                });
        }

        //[Authorize]
        public ActionResult Shift(int id = -1)
        {
            return Execute(() =>
                {
                    if (id == -1)
                        throw new ClientException("Złe ID przesunięcia.");

                    var warehouses = WarehousesService.GetWarehouses(new Request()).Data;
                    warehouses.AddRange(WarehousesService.GetPartnersWarehouses(new Request()).Data);

                    ViewBag.GroupInfo = GroupsService.GetGroupInfo(new Request<int>(id)).Data;

                    return warehouses;
                });
        }

        //[Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Shift(ShiftDto shift, int id)
        {
            return Execute(() =>
                {
                    var oldw = WarehousesService.GetWarehouseByGroup(new Request<int>(id)).Data;
                    var neww = WarehousesService.GetWarehouse(new Request<int>(shift.WarehouseId)).Data;

                    shift.Date = DateTime.Now;
                    shift.GroupId = id;
                    shift.Internal = neww.Internal;
                    shift.SenderName = oldw.Name;
                    shift.WarehouseName = neww.Name;
                    shift.SenderId = oldw.Id;

                    GroupsService.AddNewShift(new Request<ShiftDto>(shift));

                    TempData["StatusMessage"] = "Przesunięcie wykonane pomyślnie.";

                    return true;
                }, "Index", "Groups");
        }

        //[Authorize]
        public ActionResult History(int id = -1)
        {
            return Execute(() =>
                {
                    if (id == -1)
                        throw new ClientException("Złe id partii.");

                    ViewBag.GroupInfo = GroupsService.GetGroupInfo(new Request<int>(id)).Data;

                    return GroupsService.GetGroupHistory(new Request<int>(id)).Data;
                });
        }
    }
}
