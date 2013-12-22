using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WMS.ServicesInterface.DTOs;
using WMS.ServicesInterface.DataContracts;
using WMS.Client.Dialogs;
using WMS.ServicesInterface;

namespace WMS.Client.Menus
{

    /// <summary>
    /// Interaction logic for WarehouseMenu.xaml
    /// </summary>
    public partial class WarehouseMenu : BaseMenu   // 3
    {
        private int warehouseId;
        private WarehouseInfoDto warehouse;
        private List<SectorDto> sectors;
        private List<int> sectorsInfo;
        private bool isLoaded;
        private List<Button> buttons;
        private ContextMenu contextMenu;
        private MainWindow mainWindow;

        public WarehouseMenu(MainWindow mainWindow, int warehouseId, string name)
            : base(mainWindow)
        {
            this.mainWindow = mainWindow;
            mainWindow.Title = "Podgląd Magazynu";
            isLoaded = false;
            this.warehouseId = warehouseId;

            mainWindow.ReloadWindow = LoadData;

            InitializeComponent();

            contextMenu = new System.Windows.Controls.ContextMenu();

            MenuItem item1 = new MenuItem();
            item1.Click += EditClick;
            item1.Header = "Edytuj...";

            MenuItem item2 = new MenuItem();
            item2.Click += DeleteClick;
            item2.Header = "Usuń...";

            contextMenu.Items.Add(item1);
            contextMenu.Items.Add(item2);

            WarehouseNameLabel.Content = String.Format("Magazyn '{0}'", name);

            LoadData();
        }

        /// <summary>
        /// Ładowanie danych
        /// </summary>
        private void LoadData()
        {
            Execute(() => WarehousesService.GetWarehouse(new Request<int>(warehouseId)), t =>
                {
                    warehouse = t.Data;

                    Execute(() => WarehousesService.GetSectors(new Request<int>(warehouseId)), x =>
                    {
                        sectors = x.Data;

                        sectorsInfo = new List<int>();

                        foreach (var s in sectors)
                            sectorsInfo.Add(s.GroupsCount);

                        isLoaded = true;
                        InitializeData();
                    });
                });
        }

        /// <summary>
        /// Wyświetlanie danych
        /// </summary>
        private void InitializeData()
        {
            if (!isLoaded)
                return;

            AddressLabel1.Content = String.Format("{0} {1}, {2} {3}",
                            warehouse.Street, warehouse.Num, warehouse.Code, warehouse.City);

            AddressLabel2.Content = String.Format("Telefon: {0}, e-mail: {1}",
                warehouse.Tel == null ? "Brak" : warehouse.Tel,
                warehouse.Mail == null ? "Brak" : warehouse.Mail);

            WarehouseNameLabel.Content = String.Format("Magazyn '{0}'", warehouse.Name);

            buttons = new List<Button>();

            int i = 0;

            foreach (var s in sectors)
            {
                Button b = new Button();
                b.Content = String.Format("{0}\n{1} / {2}", s.Number, sectorsInfo[i], s.Limit);
                b.Width = 100;
                b.Height = 100;
                b.Tag = s.Id;
                b.Margin = new Thickness(5);
                b.Click += SectorClick;
                b.ContextMenu = contextMenu;
                b.Background = sectorsInfo[i] == s.Limit ? Brushes.Red : Brushes.Green;

                buttons.Add(b);

                ++i;
            }

            LoadingLabel.Visibility = System.Windows.Visibility.Hidden;
            isLoaded = true;

            ShowData();
        }

        /// <summary>
        /// Kliknięcie sektora
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SectorClick(Object sender, RoutedEventArgs e)
        {
            int id = (int)(e.Source as Button).Tag;
            LoadNewMenu(new SectorMenu(mainWindow, id));
        }

        /// <summary>
        /// Wyświetlanie danych
        /// </summary>
        private void ShowData()
        {
            if (!isLoaded)
                return;

            SectorsGrid.Children.Clear();
            SectorsGrid.ColumnDefinitions.Clear();
            SectorsGrid.RowDefinitions.Clear();

            int n = (int)SectorsGrid.ActualWidth / 112;
            n = n != 0 ? n : 1;

            for (int i = 0; i <= n; ++i)
                SectorsGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(110) });

            SectorsGrid.ColumnDefinitions.Last().Width = new GridLength(1, GridUnitType.Star);

            for (int i = 0; i <= sectors.Count / n + 1; ++i)
                SectorsGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(110) });

            SectorsGrid.RowDefinitions.Last().Height = new GridLength(1, GridUnitType.Star);

            for (int i = 0; i < buttons.Count; ++i)
            {
                Grid.SetColumn(buttons[i], i % n);
                Grid.SetRow(buttons[i], i / n);

                SectorsGrid.Children.Add(buttons[i]);
            }
        }

        /// <summary>
        /// Usuwanie sektora
        /// </summary>
        /// <param name="id"></param>
        private void DeleteSector(int id)
        {
            Execute(() => WarehousesService.DeleteSectorIfEmpty(new Request<int>(id)).Data, t =>
                {
                    if (t)
                    {
                        MessageBox.Show("Sektor został pomyślnie usunięty!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadData();
                    }
                    else
                        MessageBox.Show("Sektor nie jest pusty!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                });
        }

        /// <summary>
        /// Usuwanie sektora
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            if (mainWindow.Permissions > PermissionLevel.Administrator)
            {
                MessageBox.Show("Brak uprawnień!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int id = (int)(((e.Source as MenuItem).Parent as ContextMenu).PlacementTarget as Button).Tag;

            if (MessageBox.Show("Czy chcesz usunąć ten sektor?", "Uwaga!",
                MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                DeleteSector(id);
        }

        /// <summary>
        /// Edycja sektora
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditClick(object sender, RoutedEventArgs e)
        {
            if (mainWindow.Permissions > PermissionLevel.Manager)
            {
                MessageBox.Show("Brak uprawnień!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int id = (int)(((e.Source as MenuItem).Parent as ContextMenu).PlacementTarget as Button).Tag;

            SectorsDialog dlg = new SectorsDialog(mainWindow, warehouseId, id);
            dlg.Show();
        }

        /// <summary>
        /// Nowy sektor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddNewButton_Click(object sender, RoutedEventArgs e)
        {
            SectorsDialog dlg = new SectorsDialog(mainWindow, warehouseId, -1);
            dlg.Show();
        }

        /// <summary>
        /// Usuwanie magazynu
        /// </summary>
        /// <param name="id"></param>
        private void DeleteWarehouse(int id)
        {
            Execute(() => WarehousesService.DeleteIfEmpty(new Request<int>(id)).Data, t =>
                {
                    if (t)
                    {
                        MessageBox.Show("Magazyn został pomyślnie usunięty!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadData();
                    }
                    else
                        MessageBox.Show("Magazyn nie jest pusty!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                });
        }

        /// <summary>
        /// Usuwanie magazynu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Czy chcesz usunąć magazyn '" + warehouse.Name + "'?", "Uwaga!",
                MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                DeleteWarehouse(warehouse.Id);
        }

        /// <summary>
        /// Edycja magazynu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            WarehouseDialog dlg = new WarehouseDialog(mainWindow, warehouseId);
            dlg.Show();
        }

        /// <summary>
        /// Ładowanie menu
        /// </summary>
        /// <param name="menu"></param>
        private void LoadNewMenu(UserControl menu)
        {
            Grid content = Parent as Grid;

            content.Children.Remove(this);
            content.Children.Add(menu);
        }

        /// <summary>
        /// Magazyny
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WarehousesButton_Click(object sender, RoutedEventArgs e)
        {
            LoadNewMenu(new WarehousesMenu(mainWindow));
        }

        /// <summary>
        /// Menu główne
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainMenuButton_Click(object sender, RoutedEventArgs e)
        {
            LoadNewMenu(new MainMenu(mainWindow));
        }

        /// <summary>
        /// Zmiana rozmiaru kontrolki
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ShowData();
        }

        /// <summary>
        /// Zablokowanie niektórych funkcji w zależności od uprawnień.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BaseMenu_Loaded(object sender, RoutedEventArgs e)
        {
            if (mainWindow.Permissions > PermissionLevel.Administrator)
            {
                AddNewButton.Visibility = System.Windows.Visibility.Hidden;
                DeleteButton.Visibility = System.Windows.Visibility.Hidden;
            }

            if (mainWindow.Permissions > PermissionLevel.Manager)
                EditButton.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
