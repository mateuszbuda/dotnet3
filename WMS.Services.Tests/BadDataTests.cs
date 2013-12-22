using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WMS.DatabaseAccess;
using WMS.ServicesInterface.DTOs;
using WMS.ServicesInterface.DataContracts;
using WMS.Services.Assemblers;
using System.Data.Entity.Infrastructure;

namespace WMS.Services.Tests
{
    /// <summary>
    /// Test przekazywania błędnych danych.
    /// </summary>
    [TestClass]
    public class BadDataTests : ServicesProvider
    {
        private readonly string longString = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";

        /// <summary>
        /// Błędne dane magazynu.
        /// </summary>
        [TestMethod, ExpectedException(typeof(DbUpdateException))]
        public void WarehouseBadDataTest()
        {
            using (var ctx = new SystemContext())
            {
                ctx.TransactionSync(tc =>
                    {
                        tc.Rollback = true;

                        warehousesService.Rollback = false;

                        var w = CreateWarehouse();
                        w.Name = longString;

                        warehousesService.AddNew(new Request<WarehouseInfoDto>(new WarehouseAssembler().ToDto(w)));

                        warehousesService.Rollback = true;
                    });
            }
        }

        /// <summary>
        /// Błędne dane produktu.
        /// </summary>
        [TestMethod, ExpectedException(typeof(DbUpdateException))]
        public void ProductBadDataTest()
        {
            using (var ctx = new SystemContext())
            {
                ctx.TransactionSync(tc =>
                {
                    tc.Rollback = true;

                    productsService.Rollback = false;

                    var w = CreateProduct();
                    w.Name = longString;

                    productsService.AddNew(new Request<ProductDto>(new ProductAssembler().ToDto(w)));

                    productsService.Rollback = true;
                });
            }
        }

        /// <summary>
        /// Błędne dane partnera.
        /// </summary>
        [TestMethod, ExpectedException(typeof(DbUpdateException))]
        public void PartnerBadDataTest()
        {
            using (var ctx = new SystemContext())
            {
                ctx.TransactionSync(tc =>
                {
                    tc.Rollback = true;

                    partnersService.Rollback = false;

                    var w = CreatePartner();
                    w.Mail = longString + longString;

                    partnersService.AddNew(new Request<PartnerDto>(new PartnerAssembler().ToDto(w)));

                    partnersService.Rollback = true;
                });
            }
        }
    }
}
