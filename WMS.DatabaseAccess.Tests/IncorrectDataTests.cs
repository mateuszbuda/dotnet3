using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity.Infrastructure;
using WMS.DatabaseAccess.Entities;

namespace WMS.DatabaseAccess.Tests
{
    /// <summary>
    /// Testy niepoprawnych danych
    /// </summary>
    [TestClass]
    public class IncorrectDataTests : DatabaseAccessProvider
    {
        private readonly string longString = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";

        /// <summary>
        /// Niepoprawne dane magazynu
        /// </summary>
        [TestMethod, ExpectedException(typeof(DbUpdateException))]
        public void Warehouse_TooLongTest()
        {
            Warehouse w = CreateWarehouse();
            w.Name = longString;

            Transaction(tc =>
            {
                tc.Entities.Warehouses.Add(w);
                tc.Entities.SaveChanges();
            });
        }

        /// <summary>
        /// Niepoprawne dane produktu
        /// </summary>
        [TestMethod, ExpectedException(typeof(DbUpdateException))]
        public void Product_TooLongTest()
        {
            Product p = CreateProduct();
            p.Name = longString;

            Transaction(tc =>
            {
                tc.Entities.Products.Add(p);
                tc.Entities.SaveChanges();
            });
        }

        /// <summary>
        /// Niepoprawne dane partnera
        /// </summary>
        [TestMethod, ExpectedException(typeof(DbUpdateException))]
        public void Partner_TooLongTest()
        {
            Partner p = CreatePartner();
            p.City = longString;

            Transaction(tc =>
            {
                tc.Entities.Partners.Add(p);
                tc.Entities.SaveChanges();
            });
        }
    }
}
