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

namespace WMS.Client.Menus
{
    /// <summary>
    /// Interaction logic for GroupMenu.xaml
    /// </summary>
    public partial class GroupMenu : BaseMenu
    {
        private MainWindow mainWindow;
        private int groupId;
        private bool isLoaded;
        private GroupDto group;
        private bool isInternal;

        private List<ProductDetailsDto> products;

        /// <summary>
        /// Inicjalizacja menu partii
        /// </summary>
        /// <param name="mainWindow">Referencja do okna głównego</param>
        /// <param name="id">ID Partii</param>
        public GroupMenu(MainWindow mainWindow, int id) : base(mainWindow)
        {
            groupId = id;
            this.mainWindow = mainWindow;
            mainWindow.Title = "Podgląd Partii";

            mainWindow.ReloadWindow = LoadData;

            InitializeComponent();

            LoadData();
        }

        /// <summary>
        /// Ładowanie danych
        /// </summary>
        private void LoadData()
        {
            Execute(() => GroupService.GetGroupInfo(new Request<int>(groupId)), t =>
                {
                    group = t.Data;
                    isInternal = t.Data.Internal;
                    Execute(() => GroupService.GetGroupDetails(new Request<int>(group.Id)), x =>
                        {
                            products = x.Data;
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

            if (!isInternal)
                SendButton.IsEnabled = false;

            LoadingLabel.Visibility = System.Windows.Visibility.Hidden;

            GroupLabel.Content = String.Format("Magazyn '{0}', Sektor #{1}, Partia #{2}", group.WarehouseName, group.SectorNumber, group.Id);

            ProductsGrid.Items.Clear();

            foreach (ProductDetailsDto p in products)
            {
                ProductsGrid.Items.Add(p);
            }

            ProductsGrid.Visibility = System.Windows.Visibility.Visible;
        }

        /// <summary>
        /// Historia partii
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            LoadNewMenu(new GroupHistoryMenu(mainWindow, groupId));
        }

        /// <summary>
        /// Wysyłanie partii
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            ShiftDialog dlg = new ShiftDialog(mainWindow, groupId);
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
        /// Menu grup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GroupsButton_Click(object sender, RoutedEventArgs e)
        {
            LoadNewMenu(new GroupsMenu(mainWindow));
        }

        /// <summary>
        /// Menu produktu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IdButtonClick(object sender, RoutedEventArgs e)
        {
            LoadNewMenu(new ProductMenu(mainWindow, (int)(sender as Button).Tag));
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
    }
}
