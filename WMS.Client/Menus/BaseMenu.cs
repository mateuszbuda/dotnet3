using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WMS.Client.Misc;
using WMS.ServicesInterface;
using WMS.ServicesInterface.DataContracts;
using WMS.ServicesInterface.ServiceContracts;

namespace WMS.Client.Menus
{
    /// <summary>
    /// Klasa bazowa Menu
    /// Tworzy dostęp do usług
    /// </summary>
    public class BaseMenu : UserControl
    {
        protected IWarehousesService WarehousesService { get; private set; }
        protected IPartnersService PartnersService { get; private set; }
        protected IProductsService ProductsService { get; private set; }
        protected IGroupsService GroupService { get; private set; }
        protected IAuthenticationService AuthenticationService { get; private set; }

        public BaseMenu(MainWindow mainWindow)
        {
            try
            {
                var warehouseChannelFactory = new ChannelFactory<IWarehousesService>("SecureBinding_IWarehousesService");
                warehouseChannelFactory.Credentials.UserName.UserName = mainWindow.Username;
                warehouseChannelFactory.Credentials.UserName.Password = mainWindow.Password;
                WarehousesService = warehouseChannelFactory.CreateChannel();

                var partnersChannelFactory = new ChannelFactory<IPartnersService>("SecureBinding_IPartnersService");
                partnersChannelFactory.Credentials.UserName.UserName = mainWindow.Username;
                partnersChannelFactory.Credentials.UserName.Password = mainWindow.Password;
                PartnersService = partnersChannelFactory.CreateChannel();

                var productsChannelFactory = new ChannelFactory<IProductsService>("SecureBinding_IProductsService");
                productsChannelFactory.Credentials.UserName.UserName = mainWindow.Username;
                productsChannelFactory.Credentials.UserName.Password = mainWindow.Password;
                ProductsService = productsChannelFactory.CreateChannel();

                var groupsChannelFactory = new ChannelFactory<IGroupsService>("SecureBinding_IGroupsService");
                groupsChannelFactory.Credentials.UserName.UserName = mainWindow.Username;
                groupsChannelFactory.Credentials.UserName.Password = mainWindow.Password;
                GroupService = groupsChannelFactory.CreateChannel();

                var usersChannelFactory = new ChannelFactory<IAuthenticationService>("SecureBinding_IAuthenticationService");
                usersChannelFactory.Credentials.UserName.UserName = mainWindow.Username;
                usersChannelFactory.Credentials.UserName.Password = mainWindow.Password;
                AuthenticationService = usersChannelFactory.CreateChannel();
            }
            catch
            {
                MessageBox.Show("Nie można połączyć się z serwerem.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Wykonanie zapytania do usługi w wątku
        /// </summary>
        /// <typeparam name="T">Typ zwracanej wartości</typeparam>
        /// <param name="action">Akcja do wykonania</param>
        /// <param name="success">Akcja do wykonania w przypadku sukcesu</param>
        /// <param name="exception">Akcja do wykonania w przypadku wyjątku</param>
        public void Execute<T>(Func<T> action, Action<T> success = null, Action<Exception> exception = null)
        {
            var ts = TaskScheduler.FromCurrentSynchronizationContext();
            var task = new Task<T>(action);

            if (success != null)
                task.ContinueWith(t => success(t.Result), new CancellationToken(), TaskContinuationOptions.OnlyOnRanToCompletion, ts);

            exception = exception != null ? exception : DefaultExceptionHandler;
            task.ContinueWith(t => exception(t.Exception), new CancellationToken(), TaskContinuationOptions.OnlyOnFaulted, ts);

            task.Start();
        }

        /// <summary>
        /// Domyślna obsługa wyjątków
        /// </summary>
        /// <param name="e"></param>
        protected void DefaultExceptionHandler(Exception e)
        {
            if (e.InnerException != null && e.InnerException.GetType() == typeof(FaultException<ServiceException>))
                MessageBox.Show((e.InnerException as FaultException<ServiceException>).Detail.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            else
                MessageBox.Show("Nieznany błąd wewnętrzny serwera.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Wykonanie zapytania do usługi w wątku
        /// </summary>
        /// <param name="action">Akcja do wykonania</param>
        /// <param name="success">Akcja do wykonania w przypadku sukcesu</param>
        /// <param name="exception">Akcja do wykonania w przypadku wyjątku</param>
        public void Execute(Action action, Action success = null, Action<Exception> exception = null)
        {
            Execute(() => { action(); return false; }, success != null ? (x => success()) : (Action<bool>)null, exception);
        }
    }
}
