using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WMS.Client.Misc;
using WMS.ServicesInterface.DataContracts;
using WMS.ServicesInterface.DTOs;

namespace WMS.Client.Dialogs
{
    /// <summary>
    /// Okno dialogowe dodawania i edycji magazynów.
    /// </summary>
    public partial class WarehouseDialog : BaseDialog   // 13
    {
        private MainWindow mainWindow;
        private int warehouseId = -1;
        private WarehouseInfoDto warehouse;

        /// <summary>
        /// Konstruktor uzywany przy edycji magazynu
        /// </summary>
        /// <param name="mainWindow"></param>
        /// <param name="id"></param>
        public WarehouseDialog(MainWindow mainWindow, int id)
            : this(mainWindow)
        {
            Header.Content = "Edytuj dane:";
            Title = "Edycja magazynu";

            warehouseId = id;

            LoadData();
        }

        /// <summary>
        /// Konstruktor używany przy tworzeniu nowego magazynu
        /// </summary>
        /// <param name="mainWindow"></param>
        public WarehouseDialog(MainWindow mainWindow) : base(mainWindow)
        {
            this.mainWindow = mainWindow;

            InitializeComponent();
            this.DataContext = new WarehouseValidationRule();

            Header.Content = "Wprowadź dane:";
            Title = "Tworzenie nowego magazynu";
        }

        /// <summary>
        /// Ładowanie danych
        /// </summary>
        private void LoadData()
        {
            Execute(() => WarehousesService.GetWarehouse(new Request<int>(warehouseId)), t =>
                {
                    warehouse = t.Data;
                    InitializeData();
                });
        }

        /// <summary>
        /// Wyświetlanie danych
        /// </summary>
        private void InitializeData()
        {
            WarehouseValidationRule rule = DataContext as WarehouseValidationRule;

            rule.Name = NameTB.Text = warehouse.Name;
            rule.City = CityTB.Text = warehouse.City;
            rule.Code = CodeTB.Text = warehouse.Code;
            rule.Street = StreetTB.Text = warehouse.Street;
            rule.Number = NumberTB.Text = warehouse.Num;
            rule.Phone = PhoneTB.Text = warehouse.Tel;
            MailTB.Text = warehouse.Mail;
        }

        /// <summary>
        /// Zapis danych i zamknięcie okna
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveClick(object sender, RoutedEventArgs e)
        {
            WarehouseInfoDto data = new WarehouseInfoDto()
            {
                Name = NameTB.Text,
                City = CityTB.Text,
                Code = CodeTB.Text,
                Street = StreetTB.Text,
                Num = NumberTB.Text,
                Tel = PhoneTB.Text,
                Mail = MailTB.Text,
                Deleted = false,
                Internal = true,
            };

            if (warehouseId == -1)
            {
                Execute(() => WarehousesService.AddNew(new Request<WarehouseInfoDto>(data)), t =>
                    {
                        mainWindow.ReloadWindow();
                        this.Close();
                    }, t =>
                    {
                        DefaultExceptionHandler(t);
                        mainWindow.ReloadWindow();
                        this.Close();
                    });
            }
            else
            {
                data.Id = warehouseId;
                data.Version = warehouse.Version;

                Execute(() => WarehousesService.Edit(new Request<WarehouseInfoDto>(data)), t =>
                {
                    mainWindow.ReloadWindow();
                    this.Close();
                }, t =>
                {
                    DefaultExceptionHandler(t);
                    mainWindow.ReloadWindow();
                    this.Close();
                });
            }
        }

        /// <summary>
        /// Zamknięcie okna bez zapisu danych
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// lol
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NameTB_LostFocus(object sender, RoutedEventArgs e)
        {
        }
    }
}
