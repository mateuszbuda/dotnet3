using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.DatabaseAccess.Entities;

namespace WMS.DatabaseAccess.Tests
{
    /// <summary>
    /// Klasa opakowująca dostęp do bazy danych
    /// </summary>
    [TestClass]
    public class DatabaseAccessProvider
    {
        /// <summary>
        /// Transakcja z wycofaniem
        /// </summary>
        /// <param name="action"></param>
        protected void Transaction(Action<TransactionContext> action)
        {
            using (var context = new SystemContext())
            {
                context.TransactionSync(tc => 
                { 
                    action(tc); 
                    tc.Rollback = true; 
                });
            }
        }

        /// <summary>
        /// Tworzenie magazynu
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
        /// Tworzenie sektora
        /// </summary>
        /// <param name="limit">Limit</param>
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
        /// Tworzenie produktu
        /// </summary>
        /// <returns>Produkt</returns>
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
        /// Tworzenie partii
        /// </summary>
        /// <returns>Partia</returns>
        protected Group CreateGroup()
        {
            return new Group()
            {
                Sector = CreateSector()
            };
        }

        /// <summary>
        /// Tworzenie szczegółów partii
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
        /// Tworzenie partnera
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
        /// Tworzenie przesunieć
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
