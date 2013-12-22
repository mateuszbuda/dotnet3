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
    /// Menu partnerów.
    /// Umożliwia podgląd i edycję danych partnerów.
    /// </summary>
    public partial class PartnersMenu : BaseMenu // 7
    {
        private MainWindow mainWindow;
        private List<PartnerSimpleDto> partners;
        private bool isLoaded;

        /// <summary>
        /// Inicjalizacja menu
        /// </summary>
        /// <param name="mainWindow">Referencja do okna głównego</param>
        public PartnersMenu(MainWindow mainWindow) : base(mainWindow)
        {
            this.mainWindow = mainWindow;
            mainWindow.Title = "Partnerzy";
            
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
            Execute(() => PartnersService.GetPartners(new Request()), t =>
                {
                    partners = t.Data;
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

            PartnersGrid.Items.Clear();

            foreach (PartnerSimpleDto p in partners)
            {
                PartnersGrid.Items.Add(p);
            }

            LoadingLabel.Visibility = System.Windows.Visibility.Hidden;
            PartnersGrid.Visibility = System.Windows.Visibility.Visible;
        }

        /// <summary>
        /// Nowy partner
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddNewButton_Click(object sender, RoutedEventArgs e)
        {
            PartnerDialog dlg = new PartnerDialog(mainWindow);
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
        /// Partner
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WarehouseNameClick(object sender, RoutedEventArgs e)
        {
            LoadNewMenu(new PartnerMenu(mainWindow, (int)(sender as Button).Tag));
        }

        /// <summary>
        /// Historia partnera
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PartnerHistoryMenuClick(object sender, RoutedEventArgs e)
        {
            if (mainWindow.Permissions > PermissionLevel.Manager)
            {
                MessageBox.Show("Brak uprawnień!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            LoadNewMenu(new PartnerHistoryMenu(mainWindow, (int)(sender as Button).Tag));
        }

        /// <summary>
        /// Edycja partnera
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PartnerEditClick(object sender, RoutedEventArgs e)
        {
            if (mainWindow.Permissions > PermissionLevel.Manager)
            {
                MessageBox.Show("Brak uprawnień!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            PartnerDialog dlg = new PartnerDialog(mainWindow, (int)(sender as Button).Tag);
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
