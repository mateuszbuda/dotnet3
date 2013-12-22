using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KellermanSoftware.CompareNetObjects;
using WMS.DatabaseAccess.Entities;

namespace WMS.DatabaseAccess.Tests
{
    /// <summary>
    /// Podstawowe testy bazy danych
    /// </summary>
    [TestClass]
    public class DatabaseBasicTests : DatabaseAccessProvider
    {
        /// <summary>
        /// Podstawowy test magazynów
        /// </summary>
        [TestMethod]
        public void Warehouse_BasicTest()
        {
            var compare = new CompareObjects();
            compare.IgnoreObjectTypes = true;
            compare.ElementsToIgnore.AddRange(new string[] { "Sectors", "Sent", "Received", "Owners", "Version" });

            Transaction(tc =>
            {
                Warehouse w = CreateWarehouse();

                tc.Entities.Warehouses.Add(w);
                tc.Entities.SaveChanges();
                tc.Entities.ObjectContext().DetachAll();
                Warehouse wc = tc.Entities.Warehouses.Find(w.Id);

                Assert.IsTrue(wc != w);
                Assert.IsTrue(compare.Compare(w, wc));

                wc.Name = "Y";
                tc.Entities.SaveChanges();
                tc.Entities.ObjectContext().DetachAll();

                wc = tc.Entities.Warehouses.Find(w.Id);

                Assert.IsTrue(wc != w);
                Assert.IsTrue(!compare.Compare(w, wc));

                w.Name = "Y";
                tc.Entities.SaveChanges();
                tc.Entities.ObjectContext().DetachAll();

                Assert.IsTrue(compare.Compare(w, wc));
            });
        }

        /// <summary>
        /// Podstawowy test sektorów
        /// </summary>
        [TestMethod]
        public void Sector_BasicTest()
        {
            var compare = new CompareObjects();
            compare.IgnoreObjectTypes = true;
            compare.ElementsToIgnore.AddRange(new string[] { "Groups", "Version" });

            Transaction(tc =>
            {
                Sector s = CreateSector();

                tc.Entities.Sectors.Add(s);
                tc.Entities.SaveChanges();
                tc.Entities.ObjectContext().DetachAll();

                Sector sc = tc.Entities.Sectors.Find(s.Id);

                Assert.IsTrue(s != sc);
                Assert.IsTrue(compare.Compare(s, sc));

                sc.Limit = 0;
                tc.Entities.SaveChanges();
                tc.Entities.ObjectContext().DetachAll();

                sc = tc.Entities.Sectors.Find(s.Id);

                Assert.IsTrue(s != sc);
                Assert.IsTrue(!compare.Compare(s, sc));

                s.Limit = 0;
                tc.Entities.SaveChanges();
                tc.Entities.ObjectContext().DetachAll();

                Assert.IsTrue(compare.Compare(s, sc));
            });
        }

        /// <summary>
        /// Podstawowy test produktów
        /// </summary>
        [TestMethod]
        public void Product_BasicTest()
        {
            var compare = new CompareObjects();
            compare.IgnoreObjectTypes = true;
            compare.ElementsToIgnore.AddRange(new string[] { "GroupsDetails", "Date", "Version" });

            Transaction(tc =>
            {
                Product p = CreateProduct();

                tc.Entities.Products.Add(p);
                tc.Entities.SaveChanges();
                tc.Entities.ObjectContext().DetachAll();

                Product pc = tc.Entities.Products.Find(p.Id);

                Assert.IsTrue(p != pc);
                Assert.IsTrue(compare.Compare(p, pc));

                pc.Price = 10M;
                tc.Entities.SaveChanges();
                tc.Entities.ObjectContext().DetachAll();

                pc = tc.Entities.Products.Find(p.Id);

                Assert.IsTrue(p != pc);
                Assert.IsTrue(!compare.Compare(p, pc));

                p.Price = 10M;
                tc.Entities.SaveChanges();
                tc.Entities.ObjectContext().DetachAll();

                Assert.IsTrue(compare.Compare(p, pc));
            });
        }

        /// <summary>
        /// Podstawowy test partnerów
        /// </summary>
        [TestMethod]
        public void Partner_BasicTest()
        {
            var compare = new CompareObjects();
            compare.IgnoreObjectTypes = true;
            compare.ElementsToIgnore.AddRange(new string[] { "Version" });

            Transaction(tc =>
            {
                Partner p = CreatePartner();

                tc.Entities.Partners.Add(p);
                tc.Entities.SaveChanges();
                tc.Entities.ObjectContext().DetachAll();

                Partner pc = tc.Entities.Partners.Find(p.Id);

                Assert.IsTrue(p != pc);
                Assert.IsTrue(compare.Compare(p, pc));

                pc.Num = "10";
                tc.Entities.SaveChanges();
                tc.Entities.ObjectContext().DetachAll();

                pc = tc.Entities.Partners.Find(p.Id);

                Assert.IsTrue(p != pc);
                Assert.IsTrue(!compare.Compare(p, pc));

                p.Num = "10";
                tc.Entities.SaveChanges();
                tc.Entities.ObjectContext().DetachAll();

                Assert.IsTrue(compare.Compare(p, pc));
            });
        }

        /// <summary>
        /// Podstawowy test partii
        /// </summary>
        [TestMethod]
        public void Group_BasicTest()
        {
            var compare = new CompareObjects();
            compare.IgnoreObjectTypes = true;
            compare.ElementsToIgnore.AddRange(new string[] { "GroupDetails", "Shfts", "Version" });

            Transaction(tc =>
            {
                Group g = CreateGroup();

                tc.Entities.Groups.Add(g);
                tc.Entities.SaveChanges();
                tc.Entities.ObjectContext().DetachAll();

                Group gc = tc.Entities.Groups.Find(g.Id);

                Assert.IsTrue(g != gc);
                Assert.IsTrue(compare.Compare(g.Sector, gc.Sector));

                gc.Sector = CreateSector(2);
                tc.Entities.SaveChanges();

                gc = tc.Entities.Groups.Find(g.Id);

                Assert.IsTrue(g != gc);
                Assert.IsTrue(!compare.Compare(g.Sector, gc.Sector));
            });
        }

        /// <summary>
        /// Podstawowy test szczegółów partii
        /// </summary>
        [TestMethod]
        public void GroupDetails_BasicTest()
        {
            var compare = new CompareObjects();
            compare.IgnoreObjectTypes = true;
            compare.ElementsToIgnore.AddRange(new string[] { "Version" });

            Transaction(tc =>
            {
                GroupDetails g = CreateGroupDetails();

                tc.Entities.GroupsDetails.Add(g);
                tc.Entities.SaveChanges();
                tc.Entities.ObjectContext().DetachAll();

                GroupDetails gc = tc.Entities.GroupsDetails.Find(g.ProductId, g.GroupId);

                Assert.IsTrue(g != gc);
                Assert.IsTrue(compare.Compare(g, gc));

                gc.Count = 10;
                tc.Entities.SaveChanges();
                tc.Entities.ObjectContext().DetachAll();

                gc = tc.Entities.GroupsDetails.Find(g.ProductId, g.GroupId);

                Assert.IsTrue(g != gc);
                Assert.IsTrue(!compare.Compare(g, gc));

                g.Count = 10;
                tc.Entities.SaveChanges();
                tc.Entities.ObjectContext().DetachAll();

                Assert.IsTrue(compare.Compare(g, gc));
            });
        }

        /// <summary>
        /// Podstawowy test przesunięć
        /// </summary>
        [TestMethod]
        public void Shift_BasicTest()
        {
            var compare = new CompareObjects();
            compare.IgnoreObjectTypes = true;
            compare.ElementsToIgnore.AddRange(new string[] { "Date", "Version" });

            Transaction(tc =>
            {
                Shift s = CreateShift();

                tc.Entities.Shifts.Add(s);
                tc.Entities.SaveChanges();
                tc.Entities.ObjectContext().DetachAll();

                Shift sc = tc.Entities.Shifts.Find(s.Id);

                Assert.IsTrue(s != sc);
                Assert.IsTrue(compare.Compare(s, sc));

                sc.Latest = false;
                tc.Entities.SaveChanges();
                tc.Entities.ObjectContext().DetachAll();

                sc = tc.Entities.Shifts.Find(s.Id);

                Assert.IsTrue(s != sc);
                Assert.IsTrue(!compare.Compare(s, sc));

                s.Latest = false;
                tc.Entities.SaveChanges();
                tc.Entities.ObjectContext().DetachAll();

                Assert.IsTrue(compare.Compare(s, sc));
            });
        }
    }
}
