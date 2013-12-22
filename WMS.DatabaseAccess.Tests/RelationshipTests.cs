using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using WMS.DatabaseAccess.Entities;

namespace WMS.DatabaseAccess.Tests
{
    /// <summary>
    /// Testy Relacji
    /// </summary>
    [TestClass]
    public class RelationshipTests : DatabaseAccessProvider
    {
        /// <summary>
        /// Test relacji partnera i jego magazynu.
        /// </summary>
        [TestMethod]
        public void Partner_Warehouse_RelationshipTest()
        {
            Warehouse w = CreateWarehouse();
            Sector s1 = CreateSector();
            s1.Warehouse = null;
            Sector s2 = CreateSector();
            s2.Warehouse = null;

            w.Sectors = new HashSet<Sector>(new Sector[] { s1, s2 });

            Transaction(tc =>
            {
                tc.Entities.Warehouses.Add(w);
                tc.Entities.SaveChanges();
                tc.Entities.ObjectContext().DetachAll();

                Warehouse wx = tc.Entities.Warehouses.Find(w.Id);
                Assert.IsTrue(wx.Sectors.Count == 2);
                Assert.AreEqual(wx.Sectors.FirstOrDefault().Id, s1.Id);
            });
        }

        /// <summary>
        /// Test relacji produktu i partii
        /// </summary>
        [TestMethod]
        public void Group_Product_RelationshipTest()
        {
            Product p = CreateProduct();
            Group g = CreateGroup();
            GroupDetails gd = CreateGroupDetails();
            gd.Product = p;
            gd.Group = g;

            Transaction(tc =>
            {
                tc.Entities.GroupsDetails.Add(gd);
                tc.Entities.SaveChanges();
                tc.Entities.ObjectContext().DetachAll();

                Group gx = (from q in tc.Entities.Groups.Include("GroupDetails.Product") where q.Id == g.Id select q).FirstOrDefault();
                Assert.IsTrue(gx.GroupDetails.Count == 1);

                Assert.AreEqual(gx.GroupDetails.FirstOrDefault().Product.Id, p.Id);
            });
        }

        /// <summary>
        /// Test relacji przesunięcia i  magazynu
        /// </summary>
        [TestMethod]
        public void Shift_Group_Sector_Warehouse_RelationshipTest()
        {
            Warehouse w = CreateWarehouse();
            Sector s = CreateSector();
            s.Warehouse = w;
            Group g = CreateGroup();
            g.Sector = s;
            Shift h = CreateShift();
            h.Group = g;
            h.Sender = CreateWarehouse();

            Transaction(tc =>
            {
                tc.Entities.Shifts.Add(h);
                tc.Entities.SaveChanges();
                tc.Entities.ObjectContext().DetachAll();

                Shift hx = (from q in tc.Entities.Shifts.Include("Group.Sector.Warehouse") where q.Id == h.Id select q).FirstOrDefault();
                Warehouse wx = (from q in tc.Entities.Warehouses.Include("Sectors.Groups.Shifts") where q.Id == w.Id select q).FirstOrDefault();
                Assert.AreEqual(wx.Sectors.FirstOrDefault().Groups.FirstOrDefault().Shifts.FirstOrDefault().Id, h.Id);
            });
        }
    }
}
