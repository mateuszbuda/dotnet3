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
    /// Dodawanie i edycja sektorów w magazynie.
    /// </summary>
    public partial class SectorsDialog : BaseDialog     // 15
    {
        private MainWindow mainWindow;
        private int sectorId = -1;
        private int warehouseId = -1;
        private SectorDto sector;
        private WarehouseInfoDto warehouse;
        private int nextSectorNumber = 1;

        /// <summary>
        /// Konstruktor inicjalizujący dane.
        /// </summary>
        /// <param name="mainWindow"></param>
        /// <param name="warehouseId">Id magazynu dla którego tworzymy/edytujemy sektor</param>
        /// <param name="sectorId">Id edytowanego sektora lub -1 jeśli tworzony jest nowy sektor</param>
        public SectorsDialog(MainWindow mainWindow, int warehouseId, int sectorId)
            : this(mainWindow)
        {
            this.warehouseId = warehouseId;

            if (sectorId != -1)
            {
                Header.Content = "Edytuj dane:";
                Title = "Edycja sektora";

                this.sectorId = sectorId;
            }

            LoadData();
        }

        /// <summary>
        /// Podstawowy konstruktor inicjalizujący podstawowe dane wspólne dla tworzenia i edycji sektora.
        /// </summary>
        /// <param name="mainWindow"></param>
        public SectorsDialog(MainWindow mainWindow) : base(mainWindow)
        {
            this.mainWindow = mainWindow;

            InitializeComponent();
            this.DataContext = new RegexValidationRule();

            Header.Content = "Wprowadź dane:";
            Title = "Tworzenie nowego sektora";
        }

        /// <summary>
        /// Ładowanie danych
        /// </summary>
        private void LoadData()
        {
            if (sectorId != -1)
            {
                Execute(() => WarehousesService.GetSector(new Request<int>(sectorId)), t =>
                    {
                        sector = t.Data;
                        InitializeData();
                    });
            }
            else
                Execute(() => WarehousesService.GetNextSectorNumber(new Request<int>(warehouseId)), t =>
                    {
                        nextSectorNumber = t.Data;
                        InitializeData();
                    });

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
            NumberTB.Text = sectorId != -1 ? sector.Number.ToString() : nextSectorNumber.ToString();
            CapacityTB.Text = sectorId != -1 ? sector.Limit.ToString() : "1";
        }

        /// <summary>
        /// Zapis danych po ich sprawdzeniu i zamknięcie okna
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveClick(object sender, RoutedEventArgs e)
        {
            (sender as Button).IsEnabled = false;

            int number = int.Parse(NumberTB.Text);

            if (CapacityTB.Text.Length > 10 || CapacityTB.Text.Length < 1)
            {
                MessageBox.Show("Wprowadź poprawną pojemność sektora.", "Uwaga");
                (sender as Button).IsEnabled = true;
                return;
            }
            int limit = 1;
            try
            {
                limit = int.Parse(CapacityTB.Text);
            }
            catch
            {
                MessageBox.Show("Wprowadź poprawną pojemność sektora.", "Uwaga");
                (sender as Button).IsEnabled = true;
                return;
            }
            if (limit < 1)
            {
                MessageBox.Show("Wprowadź poprawną pojemność sektora.", "Uwaga");
                (sender as Button).IsEnabled = true;
                return;
            }

            SectorDto s = new SectorDto()
            {
                Deleted = false,
                Limit = limit,
                Number = number,
                WarehouseId = warehouseId,
            };

            if (sectorId == -1)
            {
                Execute(() => WarehousesService.AddSector(new Request<SectorDto>(s)), t =>
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
                s.Version = sector.Version;
                s.Id = sector.Id;

                Execute(() => WarehousesService.EditSector(new Request<SectorDto>(s)), t =>
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
    }
}
