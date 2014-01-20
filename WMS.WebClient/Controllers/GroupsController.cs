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
    /// Dostarcza widoki dla stron grup i ich przesunięć
    /// </summary>
    public class GroupsController : WCFProvider
    {
        //
        // GET: /Groups/
        /// <summary>
        /// Podmenu grup
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Index()
        {
            return Execute(() =>
                {
                    return GroupsService.GetShifts(new Request()).Data;
                });
        }

        /// <summary>
        /// Szczegóły grupy (produkty i ich ilości)
        /// </summary>
        /// <param name="id">Id grupy</param>
        [Authorize]
        public ActionResult Group(int id = -1)
        {
            return Execute(() =>
                {
                    if (id == -1)
                        throw new ClientException("Partia o takim warehouseId nie istnieje.");

                    ViewBag.GroupInfo = GroupsService.GetGroupInfo(new Request<int>(id)).Data;

                    return GroupsService.GetGroupDetails(new Request<int>(id)).Data;
                });
        }

        /// <summary>
        /// Przesuwanie grupy
        /// </summary>
        /// <param name="id">Id przesuwanej grupy</param>
        [Authorize]
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

        /// <summary>
        /// PRzesuwanie grupy
        /// </summary>
        /// <param name="shift">Wykonane przesunięcie</param>
        /// <param name="id">Id przesuniętej grupy</param>
        /// <returns></returns>
        [Authorize]
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

        /// <summary>
        /// Historia grupy (przesunięcia)
        /// </summary>
        /// <param name="id">Id grupy</param>
        [Authorize]
        public ActionResult History(int id = -1)
        {
            return Execute(() =>
                {
                    if (id == -1)
                        throw new ClientException("Złe warehouseId partii.");

                    ViewBag.GroupInfo = GroupsService.GetGroupInfo(new Request<int>(id)).Data;

                    return GroupsService.GetGroupHistory(new Request<int>(id)).Data;
                });
        }

        /// <summary>
        /// Tworzenie nowej grupy
        /// </summary>
        /// <param name="id">Id grupy (domyślnie 0)</param>
        /// <returns></returns>
        [Authorize]
        public ActionResult New(int id = 0)
        {
            return Execute(() =>
                {
                    ViewBag.InternalWarehouses = WarehousesService.GetWarehouses(new Request()).Data;
                    ViewBag.ExternalWarehouses = WarehousesService.GetPartnersWarehouses(new Request()).Data;
                    ViewBag.Products = ProductsService.GetProducts(new Request()).Data;

                    return true;
                });
        }

        /// <summary>
        /// Tworzenie nowej grupy
        /// </summary>
        /// <param name="shift">Pierwsze przesunięcie nowej grupy</param>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(ShiftModel shift)
        {
            return Execute(() =>
                {
                    if (shift == null || shift.Products == null || shift.Products.Count() == 0)
                        throw new ClientException("Nie wybrano żadnego produktu!");

                    var sectorInfo = WarehousesService.GetSector(new Request<int>(shift.SectorId)).Data;
                    var warehouseInfo = WarehousesService.GetWarehouse(new Request<int>(shift.WarehouseId)).Data;
                    var senderInfo = WarehousesService.GetWarehouse(new Request<int>(shift.SenderId)).Data;

                    GroupDetailsDto group = new GroupDetailsDto()
                    {
                        Internal = true,
                        SectorId = shift.SectorId,
                        Products = new List<ProductDetailsDto>()
                    };

                    foreach (var product in shift.Products)
                    {
                        var t = group.Products.Find(x => x.Id == product.Id);

                        if (t == null)
                        {
                            group.Products.Add(new ProductDetailsDto()
                            {
                                Id = product.Id,
                                Count = product.Count,
                            });
                        }
                        else
                        {
                            t.Count += product.Count;
                        }
                    }

                    group.Products.RemoveAll(x => x.Count <= 0);

                    ShiftDto s = new ShiftDto()
                    {
                        Internal = true,
                        SenderId = shift.SenderId,
                        WarehouseId = shift.WarehouseId,
                        RecipientSectorId = shift.SectorId
                    };

                    GroupsService.AddNewGroup(new Request<Tuple<GroupDetailsDto, ShiftDto>>(
                        new Tuple<GroupDetailsDto, ShiftDto>(group, s)));

                    TempData["StatusMessage"] = "Nowa partia dodana pomyślnie.";
                    return true;
                }, "Index");
        }

        /// <summary>
        /// Wycofywanie ostatniego przesunięcia grupy
        /// </summary>
        /// <param name="id">Id grupy</param>
        [Authorize]
        public ActionResult Withdraw(int id = -1)
        {
            return Execute(() =>
                {
                    if (id == -1)
                        throw new ClientException("Partia o takim warehouseId nie istnieje.");

                    GroupsService.Withdraw(new Request<int>(id));

                    TempData["StatusMessage"] = "Ostatnie przesunięcie partii " + id + " zostało pomyślnie wycofane.";
                    return true;
                }, "Index");
        }
    }
}
