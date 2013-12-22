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
    /// Wyświetla informacje o produkcie.
    /// </summary>
    public partial class ProductMenu : BaseMenu // 12
    {
        private MainWindow mainWindow;
        private int productId;
        private ProductDto product;

        /// <summary>
        /// Inicjalizacja menu
        /// </summary>
        /// <param name="mainWindow">Referencja do okna głównego</param>
        /// <param name="id">Id produktu</param>
        public ProductMenu(MainWindow mainWindow, int id) : base(mainWindow)
        {
            this.mainWindow = mainWindow;
            mainWindow.Title = "Podgląd Produktu";
            productId = id;
            mainWindow.ReloadWindow = LoadData;

            InitializeComponent();

            LoadData();
        }

        /// <summary>
        /// Ładowanie danych
        /// </summary>
        private void LoadData()
        {
            Execute(() => ProductsService.GetProduct(new Request<int>(productId)), t =>
                {
                    product = t.Data;
                    InitializeData();
                });
        }

        /// <summary>
        /// Wyświetlanie danych
        /// </summary>
        private void InitializeData()
        {
            LoadingLabel.Visibility = System.Windows.Visibility.Hidden;
            ProductLabel.Content = String.Format("{0}", product.Name);

            ProductGrid.Visibility = System.Windows.Visibility.Visible;

            DateLabel.Content = product.ProductionDate == null ? "Nieznana" : product.ProductionDate.ToShortDateString();
            PriceLabel.Content = product.Price.ToString();
        }

        /// <summary>
        /// Edycja produktu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            ProductDialog dlg = new ProductDialog(mainWindow, productId);
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
        /// Produkty
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProductsButton_Click(object sender, RoutedEventArgs e)
        {
            LoadNewMenu(new ProductsMenu(mainWindow));
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
        /// Zablokowanie niektórych funkcji w zależności od uprawnień.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BaseMenu_Loaded(object sender, RoutedEventArgs e)
        {
            if (mainWindow.Permissions > PermissionLevel.Manager)
                EditButton.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
