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
    /// Dodawanie i edycja produktów.
    /// </summary>
    public partial class ProductDialog : BaseDialog     // 19
    {
        private MainWindow mainWindow;
        private int productID = -1;
        private ProductDto product;

        /// <summary>
        /// Konstruktor inicjalicujący dane.
        /// </summary>
        /// <param name="mainWindow"></param>
        /// <param name="id">Id edytowanego produktu lub -1 jśli tworzony jest nowy produkt</param>
        public ProductDialog(MainWindow mainWindow, int id)
            : base(mainWindow)
        {
            this.mainWindow = mainWindow;
            productID = id;

            InitializeComponent();
            this.DataContext = new ProductValidationRule();

            if (productID == -1)
            {
                Header.Content = "Wprowadź dane:";
                Title = "Tworzenie nowego produktu";
            }
            else
            {
                Header.Content = "Edytuj dane:";
                Title = "Edycja produktu";

                LoadData();
            }
        }

        /// <summary>
        /// Ładowanie danych
        /// </summary>
        private void LoadData()
        {
            Execute(() => ProductsService.GetProduct(new Request<int>(productID)), t =>
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
            ProductValidationRule rule = DataContext as ProductValidationRule;

            rule.Name = NameTB.Text = product.Name;
            rule.PatternPrice = PriceTB.Text = product.Price.ToString();
            DateTB.Text = product.ProductionDate.ToString();
        }

        /// <summary>
        /// Zapis danych i zamknięcie okna
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveClick(object sender, RoutedEventArgs e)
        {
            ProductDto data = new ProductDto()
            {
                Name = NameTB.Text,
                Price = decimal.Parse(PriceTB.Text),
                ProductionDate = DateTime.Parse(DateTB.Text)
            };

            if (string.IsNullOrEmpty(DateTB.Text))
            {
                MessageBox.Show("Wprowadź poprawną datę.", "Uwaga");
                (sender as Button).IsEnabled = true;
                return;
            }

            if (productID == -1)
            {
                Execute(() => ProductsService.AddNew(new Request<ProductDto>(data)), t =>
                    {
                        mainWindow.ReloadWindow();
                        this.Close();
                    }, t =>
                    {
                        DefaultExceptionHandler(t);
                        mainWindow.ReloadWindow();
                        this.Close();
                    });
            }
            else
            {
                data.Id = productID;
                data.Version = product.Version;

                Execute(() => ProductsService.Edit(new Request<ProductDto>(data)), t =>
                    {
                        mainWindow.ReloadWindow();
                        this.Close();
                    }, t =>
                    {
                        DefaultExceptionHandler(t);
                        mainWindow.ReloadWindow();
                        this.Close();
                    });
            }
        }

        /// <summary>
        /// Zamknięcie okna bez zapisu danych
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
