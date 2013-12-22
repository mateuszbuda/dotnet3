using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WMS.Client.Misc;
using WMS.ServicesInterface.DataContracts;
using WMS.ServicesInterface.DTOs;

namespace WMS.Client.Dialogs
{
    /// <summary>
    /// Przesuwanie partii
    /// </summary>
    public partial class ShiftDialog : BaseDialog
    {
        private MainWindow mainWindow;
        private GroupDto group;
        private List<WarehouseDetailsDto> warehouses;
        private List<SectorDto> secotrs;
        private bool isLoaded;
        private int groupId;

        public ShiftDialog(MainWindow mainWindow, int groupId)
            : base(mainWindow)
        {
            this.mainWindow = mainWindow;
            this.groupId = groupId;

            isLoaded = false;
            InitializeComponent();

            LoadData();
        }

        /// <summary>
        /// Ładowanie danych
        /// </summary>
        private void LoadData()
        {
            Execute(() => GroupsService.GetGroupInfo(new Request<int>(groupId)), t =>
                {
                    group = t.Data;
                    Execute(() => WarehousesService.GetWarehouses(new Request()), x =>
                        {
                            warehouses = x.Data;
                            Execute(() => WarehousesService.GetPartnersWarehouses(new Request()), y =>
                                {
                                    warehouses.AddRange(y.Data);
                                    isLoaded = true;
                                    InitializeData();
                                });
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

            Header.Content = "Przesuwanie partii " + groupId.ToString();

            foreach (WarehouseDetailsDto w in warehouses)
                if ((w.Internal == true && w.FreeSectorsCount > 0) || w.Internal == false)
                    WarehousesComboBox.Items.Add(w);
        }

        /// <summary>
        /// Zmiana magazynu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WarehousesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WarehouseDetailsDto selectedW = ((WarehouseDetailsDto)((sender as ComboBox).SelectedItem));
            int wId = selectedW != null ? selectedW.Id : -1;
            if (wId == -1)
                return;

            if (!selectedW.Internal)
            {
                SectorsComboBox.IsEnabled = false;
                Execute(() => WarehousesService.GetSectors(new Request<int>(wId)), t =>
                {
                    secotrs = t.Data;
                    LoadSectorsForChosenWarehouse(true);
                    SectorsComboBox.SelectedIndex = 0;
                });
            }
            else
            {
                SectorsComboBox.IsEnabled = true;

                Execute(() => WarehousesService.GetSectors(new Request<int>(wId)), t =>
                    {
                        secotrs = t.Data;
                        LoadSectorsForChosenWarehouse(false);
                    });
            }
        }

        /// <summary>
        /// Ładowanie sektora
        /// </summary>
        private void LoadSectorsForChosenWarehouse(bool inter)
        {
            if (secotrs == null)
                return;

            SectorsComboBox.Items.Clear();

            foreach (SectorDto s in secotrs)
                if (!s.Deleted && (s.GroupsCount < s.Limit || inter))
                    SectorsComboBox.Items.Add(s);
        }

        /// <summary>
        /// Zapis zmian
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            (sender as Button).IsEnabled = false;

            if (WarehousesComboBox.SelectedIndex < 0 || SectorsComboBox.SelectedIndex < 0)
            {
                MessageBox.Show("Wybierz miejsce docelowe przesunięcia.", "Uwaga!", MessageBoxButton.OK, MessageBoxImage.Hand);
                (sender as Button).IsEnabled = true;
                return;
            }

            ShiftDto shift = new ShiftDto()
            {
                Date = DateTime.Now,
                GroupId = groupId,
                Internal = ((WarehouseDetailsDto)WarehousesComboBox.SelectedItem).Internal,
                RecipientSectorId = ((SectorDto)SectorsComboBox.SelectedItem).Id,
                SenderName = group.WarehouseName,
                WarehouseId = ((WarehouseDetailsDto)WarehousesComboBox.SelectedItem).Id,
                WarehouseName = ((WarehouseDetailsDto)WarehousesComboBox.SelectedItem).Name,
            };

            Execute(() => WarehousesService.GetWarehouseByGroup(new Request<int>(groupId)), t =>
                {
                    shift.SenderId = t.Data.Id;
                    Execute(() => GroupsService.AddNewShift(new Request<ShiftDto>(shift)), x =>
                    {
                        mainWindow.ReloadWindow();
                        this.Close();
                    });
                });            
        }

        /// <summary>
        /// Wyjście
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
