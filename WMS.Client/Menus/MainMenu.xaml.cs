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
using WMS.ServicesInterface;
using WMS.ServicesInterface.DataContracts;
using WMS.ServicesInterface.DTOs;

namespace WMS.Client.Menus
{
    /// <summary>
    /// Główne menu alpikacji.
    /// </summary>
    public partial class MainMenu : BaseMenu // 1
    {
        private MainWindow mainWindow;
        private StatisticsDto stats;
        private bool isLoaded;

        /// <summary>
        /// Wyświetlanie statystyk
        /// </summary>
        private void LoadStats()
        {
            Execute(() => WarehousesService.GetStatistics(new Request()), t =>
                {
                    stats = t.Data;
                    isLoaded = true;
                    ShowStats();
                });
        }


        /// <summary>
        /// Wyświetlenie statystyk
        /// </summary>
        private void ShowStats()
        {
            if (!isLoaded)
                return;

            WarehousesCountInfo.Text = stats.WarehousesCount.ToString();
            ProductsCountInfo.Text = stats.ProductsCount.ToString();
            PartnersCountInfo.Text = stats.PartnersCount.ToString();
            GroupsCountInfo.Text = stats.GroupsCount.ToString();
            ShiftsCountInfo.Text = stats.ShiftsCount.ToString();
            WarehousesInfo.Text = String.Format("{0}%", stats.FIllRate);
        }

        /// <summary>
        /// Inicjalizacja głównego menu
        /// </summary>
        /// <param name="mainWindow">Referencja do okna głównego</param>
        public MainMenu(MainWindow mainWindow)
            : base(mainWindow)
        {
            this.mainWindow = mainWindow;
            mainWindow.Title = "Menu Główne";
            mainWindow.ReloadWindow = ShowStats;

            isLoaded = false;
            InitializeComponent();

            LoadStats();
        }

        /// <summary>
        /// Zmiana manu
        /// </summary>
        /// <param name="menu"></param>
        private void ChangeMenu(UserControl menu)
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
        private void ButtonWarehouses_Click(object sender, RoutedEventArgs e)
        {
            ChangeMenu(new WarehousesMenu(mainWindow));
        }

        /// <summary>
        /// Partnerzy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonPartners_Click(object sender, RoutedEventArgs e)
        {
            ChangeMenu(new PartnersMenu(mainWindow));
        }

        /// <summary>
        /// Partie
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonGroups_Click(object sender, RoutedEventArgs e)
        {
            ChangeMenu(new GroupsMenu(mainWindow));
        }

        /// <summary>
        /// Produkty
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonProducts_Click(object sender, RoutedEventArgs e)
        {
            ChangeMenu(new ProductsMenu(mainWindow));
        }

        /// <summary>
        /// Panel administratora
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonAdmin_Click(object sender, RoutedEventArgs e)
        {
            ChangeMenu(new AdminMenu(mainWindow));
        }

        private void BaseMenu_Loaded(object sender, RoutedEventArgs e)
        {
            if (mainWindow.Permissions > PermissionLevel.Administrator)
                ButtonAdmin.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
