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
    /// Interaction logic for SectorMenu.xaml
    /// </summary>
    public partial class SectorMenu : BaseMenu  // 4
    {
        private int sectorId;
        private SectorDto sector;
        private List<ShiftDto> groups;
        private bool isLoaded;
        private MainWindow mainWindow;

        /// <summary>
        /// Inicjalizacja menu
        /// </summary>
        /// <param name="mainWindow">Referencja do okna głównego</param>
        /// <param name="id">ID sektora</param>
        public SectorMenu(MainWindow mainWindow, int id)
            : base(mainWindow)
        {
            this.mainWindow = mainWindow;
            mainWindow.Title = "Podgląd Sektora";
            isLoaded = false;
            sectorId = id;
            mainWindow.ReloadWindow = LoadData;

            InitializeComponent();

            LoadData();
        }

        /// <summary>
        /// Ładowanie danych
        /// </summary>
        private void LoadData()
        {
            Execute(() => WarehousesService.GetSector(new Request<int>(sectorId)), t =>
                {
                    sector = t.Data;

                    Execute(() => GroupService.GetSectorGroups(new Request<int>(sectorId)), x =>
                        {
                            groups = x.Data;
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

            LoadingLabel.Visibility = System.Windows.Visibility.Hidden;

            WarehouseSectorLabel.Content = String.Format("Magazyn '{0}', Sektor #{1}", sector.WarehouseName, sector.Number);

            GroupsGrid.Items.Clear();

            foreach (ShiftDto g in groups)
            {
                GroupsGrid.Items.Add(new
                {
                    Id = g.Id.ToString(),
                    Date = g.Date.ToString(),
                    Name = g.SenderName,
                    Send = "Wyślij",
                });
            }

            GroupsGrid.Visibility = System.Windows.Visibility.Visible;

            CountLabel.Content = String.Format("Zajęte {0} / {1}", sector.GroupsCount, sector.Limit);

            SectorsButton.IsEnabled = true;
            isLoaded = true;
        }

        /// <summary>
        /// Wyślij partię
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendButtonClick(object sender, RoutedEventArgs e)
        {
            ShiftDialog dlg = new ShiftDialog(mainWindow, int.Parse((sender as Button).Tag as string));
            dlg.Show();
        }

        /// <summary>
        /// Znajdź nadawcę
        /// </summary>
        /// <param name="id"></param>
        private void FindSender(int id)
        {
            Execute(() => PartnersService.GetPartnerByWarehouse(new Request<int>(id)), 
                t => LoadNewMenu(new PartnerMenu(mainWindow, t.Data.Id)));
        }

        /// <summary>
        /// Nadawca
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SenderButtonClick(object sender, RoutedEventArgs e)
        {
            (sender as Button).IsEnabled = false;

            ShiftDto group = groups.FirstOrDefault(g => g.Id == int.Parse((sender as Button).Tag as string));

            Execute(() => GroupService.IsSenderInternal(new Request<ShiftDto>(group)).Data, t =>
                {
                    if (t)
                        LoadNewMenu(new WarehouseMenu(mainWindow, group.SenderId, group.SenderName));
                    else
                        FindSender(group.SenderId);
                });
        }

        /// <summary>
        /// Partia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IdButtonClick(object sender, RoutedEventArgs e)
        {
            LoadNewMenu(new GroupMenu(mainWindow, int.Parse((sender as Button).Tag as string)));
        }

        /// <summary>
        /// Edycja Sektora
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            SectorsDialog dlg = new SectorsDialog(mainWindow, sector.WarehouseId, sectorId);
            dlg.Show();
        }

        /// <summary>
        /// Usunięcie sektora
        /// </summary>
        private void DeleteSector()
        {
            Execute(() => WarehousesService.DeleteSectorIfEmpty(new Request<int>(sectorId)).Data, t =>
                {
                    if (t)
                    {
                        MessageBox.Show("Sektor został pomyślnie usunięty!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                        mainWindow.MainWindowContent.Children.Clear();
                        mainWindow.MainWindowContent.Children.Add(new WarehouseMenu(mainWindow, sector.WarehouseId, sector.WarehouseName));
                    }
                    else
                        MessageBox.Show("Sektor nie jest pusty!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                });
        }

        /// <summary>
        /// Usunięcie sektora
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Czy chcesz usunąć ten sektor?", "Uwaga!",
                MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                DeleteSector();
        }

        /// <summary>
        /// Nowa grupa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewGroupButton_Click(object sender, RoutedEventArgs e)
        {
            GroupDialog dlg = new GroupDialog(mainWindow, sectorId);
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
        /// Sektory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SectorsButton_Click(object sender, RoutedEventArgs e)
        {
            LoadNewMenu(new WarehouseMenu(mainWindow, sector.WarehouseId, sector.WarehouseName));
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
            if(mainWindow.Permissions > PermissionLevel.Administrator)
                DeleteButton.Visibility = System.Windows.Visibility.Hidden;

            if (mainWindow.Permissions > PermissionLevel.Manager)
                EditButton.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
