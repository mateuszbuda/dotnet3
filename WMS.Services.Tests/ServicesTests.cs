using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WMS.ServicesInterface.DataContracts;
using WMS.DatabaseAccess;
using WMS.Services.Assemblers;
using KellermanSoftware.CompareNetObjects;
using System.Linq;

namespace WMS.Services.Tests
{
    /// <summary>
    /// Testy usług.
    /// </summary>
    [TestClass]
    public class ServicesTests : ServicesProvider
    {
        /// <summary>
        /// Test pobierania magazynów.
        /// </summary>
        [TestMethod]
        public void GetWarehousesTest()
        {
            var compare = new CompareObjects();
            compare.IgnoreObjectTypes = true;
            compare.ElementsToInclude.AddRange(new String[] { "Name", "Id" });

            using (var ctx = new SystemContext())
            {
                ctx.TransactionSync(tc =>
                    {
                        tc.Rollback = true;
                        var x = CreateWarehouse();
                        tc.Entities.Warehouses.Add(x);
                        tc.Entities.SaveChanges();

                        var y = warehousesService.GetWarehouses(new Request());

                        var z = y.Data.Find(t => t.Id == x.Id);

                        Assert.IsTrue(compare.Compare(z, x));
                    });
            }
        }

        /// <summary>
        /// Test pobierania określonego magazynu.
        /// </summary>
        [TestMethod]
        public void GetWarehouseTest()
        {
            var compare = new CompareObjects();
            compare.IgnoreObjectTypes = true;
            compare.ElementsToInclude.AddRange(new String[] 
            { 
                "Name", 
                "Id", 
                "City",
                "Code",
                "Deleted",
                "Internal",
                "Mail",
                "Num",
                "Street",
                "Tel"
            });

            using (var ctx = new SystemContext())
            {
                ctx.TransactionSync(tc =>
                {
                    tc.Rollback = true;
                    var x = CreateWarehouse();
                    tc.Entities.Warehouses.Add(x);
                    tc.Entities.SaveChanges();

                    var y = warehousesService.GetWarehouse(new Request<int>(x.Id));

                    Assert.IsTrue(compare.Compare(y, x));
                });
            }
        }

        /// <summary>
        /// Test dodawania i usuwania magazynu.
        /// </summary>
        [TestMethod]
        public void WarehouseAddDeleteTest()
        {
            using (var ctx = new SystemContext())
            {
                ctx.TransactionSync(tc =>
                    {
                        tc.Rollback = true;

                        warehousesService.Rollback = false;

                        var x = CreateWarehouse();
                        int id = warehousesService.AddNew(new Request<ServicesInterface.DTOs.WarehouseInfoDto>(new WarehouseAssembler().ToDto(x))).Data.Id;

                        var z = warehousesService.GetWarehouses(new Request()).Data.Find(t => t.Id == id);
                        Assert.IsNotNull(z);

                        warehousesService.DeleteIfEmpty(new Request<int>(id));

                        Assert.IsNull(warehousesService.GetWarehouses(new Request()).Data.Find(t => t.Id == id));

                        warehousesService.Rollback = true;
                    });
            }
        }

        /// <summary>
        /// Test edycji magazynu.
        /// </summary>
        [TestMethod]
        public void WarehouseEditTest()
        {
            using (var ctx = new SystemContext())
            {
                ctx.TransactionSync(tc =>
                        {
                            tc.Rollback = true;

                            warehousesService.Rollback = false;
                            var x = CreateWarehouse();
                            int id = warehousesService.AddNew(new Request<ServicesInterface.DTOs.WarehouseInfoDto>(new WarehouseAssembler().ToDto(x))).Data.Id;
                            var w = warehousesService.GetWarehouse(new Request<int>(id));
                            w.Data.Name = "Warehouse2";

                            warehousesService.Edit(new Request<ServicesInterface.DTOs.WarehouseInfoDto>(w.Data));
                            var z = warehousesService.GetWarehouse(new Request<int>(id));

                            Assert.IsTrue(z.Data.Name == w.Data.Name);
                            Assert.IsFalse(z.Data.Name == x.Name);

                            warehousesService.Rollback = true;
                        });
            }
        }

        /// <summary>
        /// Test pobierania produktu.
        /// </summary>
        [TestMethod]
        public void GetProductTest()
        {
            var compare = new CompareObjects();
            compare.IgnoreObjectTypes = true;
            compare.ElementsToInclude.AddRange(new String[] 
            { 
                "Id",
                "Name",
                "Price"
            });

            using (var ctx = new SystemContext())
            {
                ctx.TransactionSync(tc =>
                {
                    tc.Rollback = true;
                    var x = CreateProduct();
                    tc.Entities.Products.Add(x);
                    tc.Entities.SaveChanges();

                    var y = productsService.GetProduct(new Request<int>(x.Id));

                    Assert.IsTrue(compare.Compare(y, x));
                });
            }
        }

        /// <summary>
        /// Test pobierania partnera.
        /// </summary>
        [TestMethod]
        public void GetPartnerTest()
        {
            var compare = new CompareObjects();
            compare.IgnoreObjectTypes = true;
            compare.ElementsToInclude.AddRange(new String[] 
            { 
                "Id",
                "Name",
                "City",
                "Code",
                "Street",
                "Num",
                "Tel",
                "Mail"
            });

            using (var ctx = new SystemContext())
            {
                ctx.TransactionSync(tc =>
                {
                    tc.Rollback = true;
                    var x = CreatePartner();
                    tc.Entities.Partners.Add(x);
                    tc.Entities.SaveChanges();

                    var y = partnersService.GetPartner(new Request<int>(x.Id));

                    Assert.IsTrue(compare.Compare(y, x));
                });
            }
        }

        /// <summary>
        /// Test dodawania i aktualizacji partnera.
        /// </summary>
        [TestMethod]
        public void AddUpdatePartner()
        {
            using (var ctx = new SystemContext())
            {
                ctx.TransactionSync(tc =>
                        {
                            tc.Rollback = true;

                            partnersService.Rollback = false;

                            var x = CreatePartner();

                            var p = partnersService.AddNew(new Request<ServicesInterface.DTOs.PartnerDto>(new PartnerAssembler().ToDto(x))).Data;

                            p.City = p.City + "q";

                            var r = partnersService.Update(new Request<ServicesInterface.DTOs.PartnerDto>(p));

                            Assert.IsTrue(r.Data.City == p.City);
                            Assert.IsFalse(r.Data.City == x.City);

                            partnersService.Rollback = true;
                        });
            }
        }

        /// <summary>
        /// Pobieranie partnera na podstawie jego magazynu.
        /// </summary>
        [TestMethod]
        public void GetPartnerByWarehouse()
        {
            var compare = new CompareObjects();
            compare.IgnoreObjectTypes = true;
            compare.ElementsToInclude.AddRange(new String[] 
            { 
                "Id",
                "Name",
                "City",
                "Code",
                "Street",
                "Num",
                "Tel",
                "Mail"
            });

            using (var ctx = new SystemContext())
            {
                ctx.TransactionSync(tc =>
                {
                    tc.Rollback = true;

                    partnersService.Rollback = false;

                    var x = CreatePartner();

                    var p = partnersService.AddNew(new Request<ServicesInterface.DTOs.PartnerDto>(new PartnerAssembler().ToDto(x))).Data;
                    var w = p.Warehouse.Id;

                    var r = partnersService.GetPartnerByWarehouse(new Request<int>(w));

                    Assert.IsTrue(compare.Compare(r.Data, p));

                    partnersService.Rollback = true;
                });
            }
        }
    }
}
