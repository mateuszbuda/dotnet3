using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WMS.Services.Assemblers;
using KellermanSoftware.CompareNetObjects;

namespace WMS.Services.Tests
{
    /// <summary>
    /// Testy konwersji pomiędzy DTO a encjami.
    /// </summary>
    [TestClass]
    public class AssemblersTests : ServicesProvider
    {
        /// <summary>
        /// Test WarehouseAssembler
        /// </summary>
        [TestMethod]
        public void WarehouseAssemblerTest()
        {
            var compare = new CompareObjects();
            compare.IgnoreObjectTypes = true;

            var w = CreateWarehouse();

            var asm = new WarehouseAssembler();

            var x = asm.ToEntity(asm.ToDto(w));

            Assert.IsTrue(compare.Compare(x, w));
        }

        /// <summary>
        /// Test PartnerAssembler
        /// </summary>
        [TestMethod]
        public void PartnerAssemblerTest()
        {
            var compare = new CompareObjects();
            compare.IgnoreObjectTypes = true;
            compare.ElementsToIgnore.Add("Warehouse");

            var w = CreatePartner();

            var asm = new PartnerAssembler();

            var x = asm.ToEntity(asm.ToDto(w));

            Assert.IsTrue(compare.Compare(x, w));
        }

        /// <summary>
        /// Test ProductAssembler
        /// </summary>
        [TestMethod]
        public void ProductAssemblerTest()
        {
            var compare = new CompareObjects();
            compare.IgnoreObjectTypes = true;

            var w = CreateProduct();

            var asm = new ProductAssembler();

            var x = asm.ToEntity(asm.ToDto(w));

            Assert.IsTrue(compare.Compare(x, w));
        }

        /// <summary>
        /// Test SectorAssembler
        /// </summary>
        [TestMethod]
        public void SectorAssemblerTest()
        {
            var compare = new CompareObjects();
            compare.IgnoreObjectTypes = true;
            compare.ElementsToIgnore.Add("Warehouse");

            var w = CreateSector();

            var asm = new SectorAssembler();

            var x = asm.ToEntity(asm.ToDto(w));

            Assert.IsTrue(compare.Compare(x, w));
        }
    }
}
