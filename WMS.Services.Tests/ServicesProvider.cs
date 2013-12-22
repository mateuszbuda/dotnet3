using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WMS.DatabaseAccess.Entities;
using WMS.ServicesInterface;
using WMS.ServicesInterface.DataContracts;

namespace WMS.Services.Tests
{
    /// <summary>
    /// Klasa bazowa udostępniająca usługi i metody ułatwiające tworzenie testowych encji.
    /// </summary>
    [TestClass]
    public class ServicesProvider
    {
        protected WarehousesService warehousesService;
        protected ProductsService productsService;
        protected PartnersService partnersService;
        protected GroupsService groupsService;
        protected AuthenticationService authenticationService;

        protected PermissionLevel Level { get; set; }

        /// <summary>
        /// Metoda testująca uprawnienia. Używana w testach.
        /// </summary>
        /// <param name="level">Poziom uprawnień</param>
        private void PermissionChecker(PermissionLevel level)
        {
            if (Level > level)
                throw new FaultException<ServiceException>(new ServiceException("error"));
        }

        protected ServicesProvider()
        {
            warehousesService = new WarehousesService();
            productsService = new ProductsService();
            partnersService = new PartnersService();
            groupsService = new GroupsService();
            authenticationService = new AuthenticationService();

            warehousesService.Rollback = true;
            productsService.Rollback = true;
            partnersService.Rollback = true;
            groupsService.Rollback = true;
            authenticationService.Rollback = true;

            warehousesService.CheckPermissions = PermissionChecker;
            productsService.CheckPermissions = PermissionChecker;
            partnersService.CheckPermissions = PermissionChecker;
            groupsService.CheckPermissions = PermissionChecker;
            authenticationService.CheckPermissions = PermissionChecker;

            Level = PermissionLevel.Administrator;
        }

        /// <summary>
        /// Tworzenie magazynu.
        /// </summary>
        /// <returns>Magazyn</returns>
        protected Warehouse CreateWarehouse()
        {
            return new Warehouse()
            {
                Name = "Warehouse",
                City = "X",
                Code = "00-000",
                Deleted = false,
                Internal = true,
                Mail = "test@test.com",
                Num = "123A",
                Street = "X",
                Tel = "+48000000000"
            };
        }

        /// <summary>
        /// Tworzenie sektora i magazynu
        /// </summary>
        /// <param name="limit">Limit sektora</param>
        /// <returns>Sektor</returns>
        protected Sector CreateSector(int limit = 1)
        {
            return new Sector()
            {
                Deleted = false,
                Limit = limit,
                Number = 1,
                Warehouse = CreateWarehouse()
            };
        }

        /// <summary>
        /// Tworzy testowy produkt.
        /// </summary>
        /// <returns>Testowy produkt</returns>
        protected Product CreateProduct()
        {
            return new Product()
            {
                Name = "N",
                Date = DateTime.Now,
                Price = 12M
            };
        }

        /// <summary>
        /// Tworzenie testowej grupy
        /// </summary>
        /// <returns>Grupa</returns>
        protected Group CreateGroup()
        {
            return new Group()
            {
                Sector = CreateSector()
            };
        }

        /// <summary>
        /// Tworzenie partii i produktów.
        /// </summary>
        /// <returns>Szczegóły partii</returns>
        protected GroupDetails CreateGroupDetails()
        {
            return new GroupDetails()
            {
                Count = 1,
                Product = CreateProduct(),
                Group = CreateGroup()
            };
        }

        /// <summary>
        /// Tworzenie testowego partnera.
        /// </summary>
        /// <returns>Partner</returns>
        protected Partner CreatePartner()
        {
            Partner p = new Partner()
            {
                Tel = "0",
                City = "a",
                Code = "0",
                Mail = "a",
                Num = "0",
                Street = "a",
                Warehouse = CreateWarehouse()
            };

            p.Warehouse.Sectors = new HashSet<Sector>();
            Sector s = CreateSector(0);
            s.Warehouse = null;
            p.Warehouse.Sectors.Add(s);

            return p;
        }

        /// <summary>
        /// Nowe przesunięcie.
        /// </summary>
        /// <returns>Przesunięcie</returns>
        protected Shift CreateShift()
        {
            return new Shift()
            {
                Date = DateTime.Now,
                Group = CreateGroup(),
                Sender = CreateWarehouse(),
                Latest = true
            };
        }
    }
}
