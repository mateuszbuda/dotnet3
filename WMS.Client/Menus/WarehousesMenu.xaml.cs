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
    /// Menu Magazynów.
    /// Wyświetla wszystkie magazyny w systemie.
    /// </summary>
    public partial class WarehousesMenu : BaseMenu   // 2
    {
        private MainWindow mainWindow;

        private List<Button> buttons;
        private List<WarehouseDetailsDto> warehouses;
        private bool isLoaded;
        private ContextMenu contextMenu;

        /// <summary>
        /// Ładowanie danych
        /// </summary>
        private void LoadWarehouses()
        {
            Execute(() => WarehousesService.GetWarehouses(new Request()), x =>
            {
                warehouses = x.Data;
                isLoaded = true;
                InitializeButtons();
            });
        }

        /// <summary>
        /// Magazyn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WarehouseClick(Object sender, RoutedEventArgs e)
        {
            int id = (int)(e.Source as Button).Tag;
            string name = (from w in warehouses
                           where w.Id == id
                           select w.Name).FirstOrDefault();

            Grid content = Parent as Grid;

            content.Children.Remove(this);
            content.Children.Add(new WarehouseMenu(mainWindow, id, name));
        }

        /// <summary>
        /// Inicjalizacja danych
        /// </summary>
        private void InitializeButtons()
        {
            buttons = new List<Button>();

            foreach (WarehouseDetailsDto w in warehouses)
            {
                Button b = new Button();
                b.Content = String.Format("{0}\n{1} / {2}", w.Name, w.FreeSectorsCount, w.SectorsCount);
                b.Width = 100;
                b.Height = 100;
                b.Tag = w.Id;
                b.Margin = new Thickness(5);
                b.Click += WarehouseClick;
                b.ContextMenu = contextMenu;
                b.Background = w.FreeSectorsCount == 0 ? (w.SectorsCount == 0 ? Brushes.Silver : Brushes.Red) : Brushes.Green;

                buttons.Add(b);
            }

            LoadingLabel.Visibility = System.Windows.Visibility.Hidden;
            ShowButtons();
        }

        /// <summary>
        /// Inicjalizacja manu
        /// </summary>
        /// <param name="mainWindow">Referencja do okna głównego</param>
        public WarehousesMenu(MainWindow mainWindow) : base(mainWindow)
        {
            this.mainWindow = mainWindow;
            mainWindow.Title = "Magazyny";
            mainWindow.ReloadWindow = LoadWarehouses;

            isLoaded = false;
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

            warehouses = new List<WarehouseDetailsDto>();

            LoadWarehouses();
        }

        /// <summary>
        /// Usunięcie magazynu
        /// </summary>
        /// <param name="id">Id magazynu</param>
        private void DeleteWarehouse(int id)
        {
            Execute(() => WarehousesService.DeleteIfEmpty(new Request<int>(id)).Data, t =>
                {
                    if (t)
                    {
                        MessageBox.Show("Magazyn został pomyślnie usunięty!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadWarehouses();
                    }
                    else
                        MessageBox.Show("Magazyn nie jest pusty!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                });
        }

        /// <summary>
        /// Usunięcie magazynu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DeleteClick(object sender, RoutedEventArgs e)
        {
            if (mainWindow.Permissions > PermissionLevel.Administrator)
            {
                MessageBox.Show("Brak uprawnień!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int id = (int)(((e.Source as MenuItem).Parent as ContextMenu).PlacementTarget as Button).Tag;
            string name = (from w in warehouses
                           where w.Id == id
                           select w.Name).FirstOrDefault();

            if (MessageBox.Show("Czy chcesz usunąć magazyn '" + name + "'?", "Uwaga!",
                MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                DeleteWarehouse(id);
        }

        /// <summary>
        /// Edycja magazynu
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

            WarehouseDialog dlg = new WarehouseDialog(mainWindow, id);
            dlg.Show();
        }

        /// <summary>
        /// Menu główne
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainMenuButton_Click(object sender, RoutedEventArgs e)
        {
            Grid content = Parent as Grid;

            content.Children.Remove(this);
            content.Children.Add(new MainMenu(mainWindow));
        }

        /// <summary>
        /// Wyświetlenie danych
        /// </summary>
        private void ShowButtons()
        {
            if (!isLoaded)
                return;

            WarehousesGrid.ColumnDefinitions.Clear();
            WarehousesGrid.RowDefinitions.Clear();
            WarehousesGrid.Children.Clear();

            int n = (int)WarehousesGrid.ActualWidth / 112;

            for (int i = 0; i <= n; ++i)
                WarehousesGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(110) });

            WarehousesGrid.ColumnDefinitions.Last().Width = new GridLength(1, GridUnitType.Star);

            for (int i = 0; i <= warehouses.Count / n + 1; ++i)
                WarehousesGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(110) });

            WarehousesGrid.RowDefinitions.Last().Height = new GridLength(1, GridUnitType.Star);

            for (int i = 0; i < buttons.Count; ++i)
            {
                Grid.SetColumn(buttons[i], i % n);
                Grid.SetRow(buttons[i], i / n);

                WarehousesGrid.Children.Add(buttons[i]);
            }
        }

        /// <summary>
        /// Nowy magazyn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddNewButton_Click(object sender, RoutedEventArgs e)
        {
            WarehouseDialog dlg = new WarehouseDialog(mainWindow);
            dlg.Show();
        }

        /// <summary>
        /// Zmiana rozmiaru kontrolki
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ShowButtons();
        }

        /// <summary>
        /// Zablokowanie niektórych funkcji w zależności od uprawnień.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BaseMenu_Loaded(object sender, RoutedEventArgs e)
        {
            if (mainWindow.Permissions > PermissionLevel.Administrator)
                AddNewButton.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
