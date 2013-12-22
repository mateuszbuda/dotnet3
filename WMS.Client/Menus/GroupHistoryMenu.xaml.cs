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
    /// Interaction logic for GroupHistoryMenu.xaml
    /// </summary>
    public partial class GroupHistoryMenu : BaseMenu
    {
        private int groupId;
        private MainWindow mainWindow;
        private List<ShiftDto> history;
        private bool isLoaded;

        /// <summary>
        /// Inicjalizacja menu historii grupy.
        /// </summary>
        /// <param name="mainWindow">Referencja do okna głównego</param>
        /// <param name="id">ID Grupy</param>
        public GroupHistoryMenu(MainWindow mainWindow, int id)
            : base(mainWindow)
        {
            groupId = id;
            this.mainWindow = mainWindow;
            mainWindow.Title = "Historia Partii";
            mainWindow.ReloadWindow = LoadData;

            InitializeComponent();

            GroupLabel.Content = String.Format("Historia partii #{0}", groupId);
            GroupButton.Content = String.Format("Partia #{0}", groupId);

            LoadData();
        }

        /// <summary>
        /// Ładowanie danych
        /// </summary>
        private void LoadData()
        {
            Execute(() => GroupService.GetGroupHistory(new Request<int>(groupId)), t =>
                {
                    history = t.Data;
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

            LoadingLabel.Visibility = System.Windows.Visibility.Hidden;

            HistoryGrid.Items.Clear();

            foreach (ShiftDto h in history)
            {
                HistoryGrid.Items.Add(h);
            }

            HistoryGrid.Visibility = System.Windows.Visibility.Visible;
        }

        /// <summary>
        /// Przejście do menu partnera/magazynu
        /// </summary>
        /// <param name="id">ID (chyba) magazynu nadawcy/odbiorcy</param>
        /// <param name="sender">Nadawca czy odbiorca</param>
        private void RecipientSenderClick(int id, bool sender)
        {
            int realId = id;
            string name = null;

            Execute(() => WarehousesService.GetWarehouse(new Request<int>(id)), t =>
                {

                    if (t.Data.Internal)
                    {
                        name = t.Data.Name;
                        LoadNewMenu(new WarehouseMenu(mainWindow, realId, name));
                    }
                    else
                        Execute(() => PartnersService.GetPartnerByWarehouse(new Request<int>(id)), x =>
                        {
                            realId = x.Data.Id;
                            LoadNewMenu(new PartnerMenu(mainWindow, realId));
                        });
                });
        }

        /// <summary>
        /// Nadawca
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SenderButtonClick(object sender, RoutedEventArgs e)
        {
            int id = (int)(sender as Button).Tag;
            (sender as Button).IsEnabled = false;

            RecipientSenderClick(id, true);
        }

        /// <summary>
        /// Odbiorca
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RecipientButtonClick(object sender, RoutedEventArgs e)
        {
            int id = (int)(sender as Button).Tag;
            (sender as Button).IsEnabled = false;

            RecipientSenderClick(id, false);
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
        /// Powrót do grupy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GroupButton_Click(object sender, RoutedEventArgs e)
        {
            LoadNewMenu(new GroupMenu(mainWindow, groupId));
        }

        /// <summary>
        /// Ładowanie manu
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
