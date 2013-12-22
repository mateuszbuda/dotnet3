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
using WMS.Client.Dialogs;
using WMS.ServicesInterface;
using WMS.ServicesInterface.DataContracts;
using WMS.ServicesInterface.DTOs;

namespace WMS.Client.Menus
{
    /// <summary>
    /// Podgląd i edycja produktów.
    /// </summary>
    public partial class ProductsMenu : BaseMenu     // 11
    {
        private MainWindow mainWindow;
        private List<ProductDto> products;
        private bool isLoaded;

        /// <summary>
        /// Inicjalizacja menu
        /// </summary>
        /// <param name="mainWindow">Referencja do okna głównego</param>
        public ProductsMenu(MainWindow mainWindow) : base(mainWindow)
        {
            this.mainWindow = mainWindow;
            mainWindow.Title = "Produkty";

            mainWindow.ReloadWindow = LoadData;

            isLoaded = false;
            InitializeComponent();

            LoadData();
        }

        /// <summary>
        /// Ładowanie danych
        /// </summary>
        private void LoadData()
        {
            Execute(() => ProductsService.GetProducts(new Request()), t =>
                {
                    products = t.Data;
                    isLoaded = true;
                    InitializeData();
                });
        }

        /// <summary>
        /// Wyświetlanie danych
        /// </summary>
        private void InitializeData()
        {
            if (!isLoaded)
                return;

            ProductsGrid.Items.Clear();

            foreach (ProductDto p in products)
            {
                ProductsGrid.Items.Add(p);
            }

            LoadingLabel.Visibility = System.Windows.Visibility.Hidden;
            ProductsGrid.Visibility = System.Windows.Visibility.Visible;
        }

        /// <summary>
        /// Nowy produkt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddNewButton_Click(object sender, RoutedEventArgs e)
        {
            ProductDialog dlg = new ProductDialog(mainWindow, -1);
            dlg.Show();
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
        /// Produkt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProductNameClick(object sender, RoutedEventArgs e)
        {
            LoadNewMenu(new ProductMenu(mainWindow, (int)(sender as Button).Tag));
        }

        /// <summary>
        /// Edycja produktu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditProductClick(object sender, RoutedEventArgs e)
        {
            if (mainWindow.Permissions > PermissionLevel.Manager)
            {
                MessageBox.Show("Brak uprawnień!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ProductDialog dlg = new ProductDialog(mainWindow, (int)(sender as Button).Tag);
            dlg.Show();
        }

        /// <summary>
        /// Zablokowanie niektórych funkcji w zależności od uprawnień.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BaseMenu_Loaded(object sender, RoutedEventArgs e)
        {
            if (mainWindow.Permissions > PermissionLevel.Manager)
                AddNewButton.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
