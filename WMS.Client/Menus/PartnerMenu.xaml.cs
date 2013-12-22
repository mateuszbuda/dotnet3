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
    /// Podgląd danych partnera.
    /// </summary>
    public partial class PartnerMenu : BaseMenu  // 8
    {
        private MainWindow mainWindow;
        private int partnerId;
        private PartnerDto partner;

        /// <summary>
        /// Inicjalizacja menu
        /// </summary>
        /// <param name="mainWindow">Referencja do okna głównego</param>
        /// <param name="id">ID partnera</param>
        public PartnerMenu(MainWindow mainWindow, int id) : base(mainWindow)
        {
            this.mainWindow = mainWindow;
            mainWindow.Title = "Podgląd Partnera";
            partnerId = id;
            mainWindow.ReloadWindow = LoadData;

            InitializeComponent();

            LoadData();
        }

        /// <summary>
        /// Ładowanie danych
        /// </summary>
        private void LoadData()
        {
            Execute(() => PartnersService.GetPartner(new Request<int>(partnerId)), t =>
                {
                    partner = t.Data;
                    InitializeData();
                });
        }

        /// <summary>
        /// Wyświetlanie danych
        /// </summary>
        private void InitializeData()
        {
            LoadingLabel.Visibility = System.Windows.Visibility.Hidden;
            PartnerLabel.Content = String.Format("Partner {0}", partner.Warehouse.Name);

            PartnerGrid.Visibility = System.Windows.Visibility.Visible;

            AddressLabel1.Content = String.Format("{0} {1}\n{2} {3}", partner.Street, partner.Num, partner.Code, partner.City);
            TelLabel1.Content = partner.Tel;
            MailLabel1.Content = partner.Mail == null ? "Brak" : partner.Mail;

            AddressLabel2.Content = String.Format("{0} {1}\n{2} {3}", partner.Warehouse.Street,
                partner.Warehouse.Num, partner.Warehouse.Code, partner.Warehouse.City);
            TelLabel2.Content = partner.Warehouse.Tel == null ? "Brak" : partner.Warehouse.Tel;
            MailLabel2.Content = partner.Warehouse.Mail == null ? "Brak" : partner.Warehouse.Mail;
        }

        /// <summary>
        /// Partnerzy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PartnersButton_Click(object sender, RoutedEventArgs e)
        {
            LoadNewMenu(new PartnersMenu(mainWindow));
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
        /// Edycja partnera
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            PartnerDialog dlg = new PartnerDialog(mainWindow, partnerId);
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
        /// Historia partnera
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            LoadNewMenu(new PartnerHistoryMenu(mainWindow, partnerId));
        }

        /// <summary>
        /// Zablokowanie niektórych funkcji w zależności od uprawnień.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BaseMenu_Loaded(object sender, RoutedEventArgs e)
        {
            if (mainWindow.Permissions > PermissionLevel.Manager)
            {
                EditButton.Visibility = System.Windows.Visibility.Hidden;
                HistoryButton.Visibility = System.Windows.Visibility.Hidden;
            }
        }
    }
}
