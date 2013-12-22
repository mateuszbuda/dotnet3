using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using WMS.DatabaseAccess;
using WMS.DatabaseAccess.Entities;
using WMS.Services.Assemblers;
using WMS.ServicesInterface;
using WMS.ServicesInterface.DataContracts;

namespace WMS.Services
{
    public delegate void CheckPermissionsDelegate(PermissionLevel permission);

    /// <summary>
    /// Bazowa klasa dla serwisów
    /// </summary>
    public class ServiceBase
    {
        /// <summary>
        /// Konwerer danych produktu
        /// </summary>
        protected ProductAssembler productAssembler;
        /// <summary>
        /// Konwerter danych uzytkownika
        /// </summary>
        protected UserAssembler userAssembler;
        /// <summary>
        /// Konwerter danych partnera
        /// </summary>
        protected PartnerAssembler partnerAssembler;
        /// <summary>
        /// Konwerter danych partii
        /// </summary>
        protected GroupAssembler groupAssembler;
        /// <summary>
        /// Konwerter danych magazynu
        /// </summary>
        protected WarehouseAssembler warehouseAssembler;
        /// <summary>
        /// Konwerter danych sektora
        /// </summary>
        protected SectorAssembler sectorAssembler;

        /// <summary>
        /// Do testów, czy wycofać transakcję
        /// </summary>
        public bool Rollback { get; set; }

        /// <summary>
        /// Konstruktor inicjalizujące pola
        /// </summary>
        public ServiceBase()
        {
            productAssembler = new ProductAssembler();
            userAssembler = new UserAssembler();
            partnerAssembler = new PartnerAssembler();
            groupAssembler = new GroupAssembler();
            warehouseAssembler = new WarehouseAssembler();
            sectorAssembler = new SectorAssembler();

            CheckPermissions = DefaultPermissionChecker;
            Rollback = false;
        }

        /// <summary>
        /// Delegat sprawdzający uprawnienia użytkowników
        /// </summary>
        public CheckPermissionsDelegate CheckPermissions { get; set; }

        /// <summary>
        /// Metdoda opakowująca, wywokonująca akcję w transakcji.
        /// </summary>
        /// <typeparam name="T">Typ zwracanej wartości</typeparam>
        /// <param name="action">Akcja do wykonania</param>
        /// <returns></returns>
        protected T Transaction<T>(Func<TransactionContext, T> action)
        {
            using (var context = new SystemContext())
            {
                return context.TransactionSync(tc =>
                    {
                        if (Rollback)
                            tc.Rollback = true;
                        return action(tc);
                    });
            }
        }

        /// <summary>
        /// Metdoda opakowująca, wywokonująca akcję w transakcji.
        /// </summary>
        /// <param name="action">Akcja do wykonania</param>
        protected void Transaction(Action<TransactionContext> action)
        {
            using (var context = new SystemContext())
            {
                context.TransactionSync(tc =>
                    {
                        if (Rollback)
                            tc.Rollback = true;
                        action(tc);
                    });
            }
        }

        /// <summary>
        /// Domyślna metoda testująca uprawnienia.
        /// </summary>
        /// <param name="permission">Wymagany poziom uprawnień</param>
        protected void DefaultPermissionChecker(PermissionLevel permission)
        {
            User u = null;
            Transaction(tc => u = tc.Entities.Users.
                Where(x => x.Username == ServiceSecurityContext.Current.PrimaryIdentity.Name).FirstOrDefault());

            if (u == null || u.Permissions > (int)permission)
                throw new FaultException<ServiceException>(new ServiceException("Brak uprawnień!"));
        }
    }
}