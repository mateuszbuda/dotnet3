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
    /// Okno dialogowe dodawnia partii.
    /// </summary>
    public partial class GroupDialog : BaseDialog
    {
        private MainWindow mainWindow;
        private List<ProductDto> products;
        private List<WarehouseDetailsDto> internalOnes = new List<WarehouseDetailsDto>();
        private List<WarehouseDetailsDto> externalOnes = new List<WarehouseDetailsDto>();
        private List<SectorDto> secotrs;
        private bool isLoaded;

        public GroupDialog(MainWindow mainWindow, int sectorId)
            : base(mainWindow)
        {
            this.mainWindow = mainWindow;

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
                    Execute(() => WarehousesService.GetWarehouses(new Request()), x =>
                        {
                            internalOnes = x.Data;
                            Execute(() => WarehousesService.GetPartnersWarehouses(new Request()), y =>
                                {
                                    externalOnes = y.Data;
                                    isLoaded = true;
                                    InitializeData();
                                });
                        });
                });
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

            Execute(() => WarehousesService.GetSectors(new Request<int>(wId)), t =>
            {
                secotrs = t.Data;
                LoadSectorsForChosenWarehouse();
            });
        }

        /// <summary>
        /// Ładowanie sektora
        /// </summary>
        private void LoadSectorsForChosenWarehouse()
        {
            if (secotrs == null)
                return;

            SectorsComboBox.Items.Clear();

            foreach (SectorDto s in secotrs)
                if (!s.Deleted && s.GroupsCount < s.Limit)
                    SectorsComboBox.Items.Add(s);
        }

        /// <summary>
        /// Przygotowywanie danych do wyświetlania
        /// </summary>
        private void InitializeData()
        {
            if (!isLoaded)
                return;

            foreach (WarehouseDetailsDto w in externalOnes)
                PartnersComboBox.Items.Add(w);

            foreach (WarehouseDetailsDto w in internalOnes)
                if ((w.Internal == true && w.FreeSectorsCount > 0) || w.Internal == false)
                    WarehousesComboBox.Items.Add(w);

            ProductGroupRow productRow = new ProductGroupRow(Products);
            foreach (ProductDto p in products)
            {
                productRow.ProductsComboBox.Items.Add(p);
            }
            Products.Items.Add(productRow);
        }

        /// <summary>
        /// Dodaje nowy wiersz do wprowadzenia danych produktu w partii.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddProductClick(object sender, RoutedEventArgs e)
        {
            ProductGroupRow productRow = new ProductGroupRow(Products);
            Products.Items.Add(productRow);

            foreach (ProductDto p in products)
                productRow.ProductsComboBox.Items.Add(p);
        }

        /// <summary>
        /// Zapisuje partię, po wcześniejszym sprawdzeniu danych i zamyka okno.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            (sender as Button).IsEnabled = false;

            if (PartnersComboBox.SelectedIndex < 0 || WarehousesComboBox.SelectedIndex < 0 || SectorsComboBox.SelectedIndex < 0)
            {
                MessageBox.Show("Wypełnij poprawnie wszystkie dane.", "Uwaga");
                (sender as Button).IsEnabled = true;
                return;
            }

            GroupDetailsDto group = new GroupDetailsDto()
            {
                Internal = true,
                SectorId = ((SectorDto)SectorsComboBox.SelectedItem).Id,
                SectorNumber = ((SectorDto)SectorsComboBox.SelectedItem).Number,
                WarehouseName = ((WarehouseDetailsDto)WarehousesComboBox.SelectedItem).Name,
            };
            group.Products = new List<ProductDetailsDto>(Products.Items.Count);
            foreach (ProductGroupRow productRow in Products.Items)
            {
                if (productRow.ProductsComboBox.SelectedIndex < 0)
                {
                    MessageBox.Show("Wypełnij poprawnie wszystkie dane.", "Uwaga", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    (sender as Button).IsEnabled = true;
                    return;
                }

                ProductDto p = (ProductDto)productRow.ProductsComboBox.SelectedItem;

                if (group.Products.Find(x => x.Id == p.Id) != null)
                {
                    MessageBox.Show("Ten sam produkt został wybrany więcej niż 1 raz.", "Uwaga", MessageBoxButton.OK, MessageBoxImage.Warning);
                    (sender as Button).IsEnabled = true;
                    return;
                }

                group.Products.Add(new ProductDetailsDto()
                {
                    Count = int.Parse(productRow.Quantity.Text),
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    ProductionDate = p.ProductionDate,
                });
            }

            ShiftDto shift = new ShiftDto()
            {
                Internal = true,
                SenderId = ((WarehouseDetailsDto)PartnersComboBox.SelectedItem).Id,
                SenderName = ((WarehouseDetailsDto)PartnersComboBox.SelectedItem).Name,
                WarehouseId = ((WarehouseDetailsDto)WarehousesComboBox.SelectedItem).Id,
                WarehouseName = ((WarehouseDetailsDto)WarehousesComboBox.SelectedItem).Name,
                RecipientSectorId = ((SectorDto)SectorsComboBox.SelectedItem).Id
            };

            Execute(() => GroupsService.AddNewGroup(
                new Request<Tuple<GroupDetailsDto, ShiftDto>>(new Tuple<GroupDetailsDto, ShiftDto>(group, shift))), t =>
                {
                    mainWindow.ReloadWindow();
                    this.Close();
                });
        }

        /// <summary>
        /// Anuluje tworzenie nowej partii i zamyka okna bez zapisu danych.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
