using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.DatabaseAccess.Entities;

namespace WMS.DatabaseAccess
{
    /// <summary>
    /// Kontekst bazy danych.
    /// </summary>
    public class SystemEntities : DbContext
    {
        /// <summary>
        /// Magazyny
        /// </summary>
        public DbSet<Warehouse> Warehouses { get; set; }
        
        /// <summary>
        /// Sektory
        /// </summary>
        public DbSet<Sector> Sectors { get; set; }

        /// <summary>
        /// Partie
        /// </summary>
        public DbSet<Group> Groups { get; set; }

        /// <summary>
        /// Przesunięcia
        /// </summary>
        public DbSet<Shift> Shifts { get; set; }

        /// <summary>
        /// Produkty
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Partnerzy
        /// </summary>
        public DbSet<Partner> Partners { get; set; }

        /// <summary>
        /// Szczegóły grup
        /// </summary>
        public DbSet<GroupDetails> GroupsDetails { get; set; }

        /// <summary>
        /// Użytkownicy
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Metoda do testów jednostkowych.
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}
