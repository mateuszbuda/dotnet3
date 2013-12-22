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
using WMS.ServicesInterface.DataContracts;
using WMS.ServicesInterface.DTOs;

namespace WMS.Client.Menus
{
    /// <summary>
    /// Interaction logic for GroupsMenu.xaml
    /// </summary>
    public partial class GroupsMenu : BaseMenu // 10
    {
        private MainWindow mainWindow;
        private List<ShiftDto> groups;
        private bool isLoaded;
        private bool showInternal = false;
        private bool showExternal = false;

        public GroupsMenu(MainWindow mainWindow)
            : base(mainWindow)
        {
            this.mainWindow = mainWindow;
            mainWindow.Title = "Przetwarzane Partie";
            mainWindow.ReloadWindow = LoadData;

            isLoaded = false;
            InitializeComponent();

            showInternal = (bool)Internal.IsChecked;
            showExternal = (bool)External.IsChecked;

            LoadData();
        }

        /// <summary>
        /// Ładowanie danych
        /// </summary>
        private void LoadData()
        {
            Execute(() => GroupService.GetShifts(new Request()), t =>
                {
                    groups = t.Data;
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

            GroupsGrid.Items.Clear();

            foreach (ShiftDto g in groups)
            {
                if ((showInternal && g.Internal) || (showExternal && !g.Internal))
                    GroupsGrid.Items.Add(g);
            }

            LoadingLabel.Visibility = System.Windows.Visibility.Hidden;
            GroupsGrid.Visibility = System.Windows.Visibility.Visible;
        }

        /// <summary>
        /// Nowa partia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddNewButton_Click(object sender, RoutedEventArgs e)
        {
            GroupDialog dlg = new GroupDialog(mainWindow, 0);
            dlg.Show();
        }

        /// <summary>
        /// Manu główne
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
        /// Zmiana zaznaczenia magazynów wewnętrznych
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Internal_Checked(object sender, RoutedEventArgs e)
        {
            if (Internal != null)
            {
                showInternal = (bool)Internal.IsChecked;
                // a może tylko InitializeData() ?
                LoadData();
            }
        }

        /// <summary>
        /// Zmiana zaznaczenia magazynów wewnętrznych
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Internal_Unchecked(object sender, RoutedEventArgs e)
        {
            if (Internal != null)
            {
                showInternal = (bool)Internal.IsChecked;
                // a może tylko InitializeData() ?
                LoadData();
            }
        }

        /// <summary>
        /// Zmiana zaznaczenia magazynow partnerów
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void External_Unchecked(object sender, RoutedEventArgs e)
        {
            if (External != null)
            {
                showExternal = (bool)External.IsChecked;
                // a może tylko InitializeData() ?
                LoadData();
            }
        }

        /// <summary>
        /// Zmiana zaznaczenia magazynów partnerów
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void External_Click(object sender, RoutedEventArgs e)
        {
            if (External != null)
            {
                showExternal = (bool)External.IsChecked;
                // a może tylko InitializeData() ?
                LoadData();
            }
        }

        /// <summary>
        /// Partia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GroupIdClick(object sender, RoutedEventArgs e)
        {
            LoadNewMenu(new GroupMenu(mainWindow, (int)(sender as Button).Tag));
        }

        /// <summary>
        /// Przesuń
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShiftClick(object sender, RoutedEventArgs e)
        {
            //DatabaseAccess.Group toMove = groups.Find(delegate(DatabaseAccess.Group gr)
            //{
            //    return gr.Id == (int)((sender as Button).Tag);
            //});

            //if (toMove.InInternal())
            //{
            ShiftDialog dlg = new ShiftDialog(mainWindow, (int)((sender as Button).Tag));
            dlg.Show();
            //}
            //else
            //    MessageBox.Show("Partia znajduje się u partnera. Nie mozna przesunąć", "Uwaga");
        }

        private void FindSender(int id)
        {
            Execute(() => PartnersService.GetPartnerByWarehouse(new Request<int>(id)),
                t => LoadNewMenu(new PartnerMenu(mainWindow, t.Data.Id)));
        }

        private void WarehouseClick(object sender, RoutedEventArgs e)
        {
            int gid = (int)(sender as Button).Tag;
            var group = groups.Find(x => x.Id == gid);

            //Execute(() => GroupService.IsSenderInternal(new Request<ShiftDto>(group)).Data, t =>
            //{
                if (group.Internal)
                    LoadNewMenu(new WarehouseMenu(mainWindow, group.WarehouseId, group.WarehouseName));
                else
                    FindSender(group.WarehouseId);
            //});
            //LoadNewMenu(new WarehouseMenu(mainWindow, (int)(sender as Button).Tag, (sender as Button).Content.ToString()));
        }
    }
}
