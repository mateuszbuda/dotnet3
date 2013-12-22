using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WMS.ServicesInterface.DataContracts;
using WMS.ServicesInterface.DTOs;
using WMS.Services.Assemblers;
using System.ServiceModel;

namespace WMS.Services.Tests
{
    /// <summary>
    /// Testy uprawnień.
    /// </summary>
    [TestClass]
    public class PermissionsTest : ServicesProvider
    {
        public PermissionsTest()
        {
            Level = ServicesInterface.PermissionLevel.User;
        }

        /// <summary>
        /// Test uprawnień potrzebnych do dodawania magazynu.
        /// </summary>
        [TestMethod, ExpectedException(typeof(FaultException<ServiceException>))]
        public void WarehouseAddPermissionTest()
        {
            var w = CreateWarehouse();
            warehousesService.AddNew(new Request<WarehouseInfoDto>(new WarehouseAssembler().ToDto(w)));
        }

        /// <summary>
        /// Test uprawnień potrzebnych do dodawania sektorów.
        /// </summary>
        [TestMethod, ExpectedException(typeof(FaultException<ServiceException>))]
        public void SectorAddPermissionTest()
        {
            var w = CreateSector();
            warehousesService.AddSector(new Request<SectorDto>(new SectorAssembler().ToDto(w)));
        }

        /// <summary>
        /// Test uprawnień potrzebnych do dodawania produktów.
        /// </summary>
        [TestMethod, ExpectedException(typeof(FaultException<ServiceException>))]
        public void ProductAddPermissionTest()
        {
            var w = CreateProduct();
            productsService.AddNew(new Request<ProductDto>(new ProductAssembler().ToDto(w)));
        }

        /// <summary>
        /// Test uprawnień potrzebnych do  dodawania partnera.
        /// </summary>
        [TestMethod, ExpectedException(typeof(FaultException<ServiceException>))]
        public void PartnerAddPermissionTest()
        {
            var w = CreatePartner();
            partnersService.AddNew(new Request<PartnerDto>(new PartnerAssembler().ToDto(w)));
        }
    }
}
