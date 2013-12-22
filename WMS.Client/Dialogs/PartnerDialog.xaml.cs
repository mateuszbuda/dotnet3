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
    /// Dodawanie i edycja partnerów.
    /// </summary>
    public partial class PartnerDialog : BaseDialog     // 18
    {
        private MainWindow mainWindow;
        private int partnerId = -1;
        private PartnerDto partner;

        /// <summary>
        /// Konstruktor wykorzystywany przy edycji partnera.
        /// </summary>
        /// <param name="mainWindow"></param>
        /// <param name="id">Id edytowanego partnera</param>
        public PartnerDialog(MainWindow mainWindow, int id)
            : this(mainWindow)
        {
            Header.Content = "Edytuj dane:";
            Title = "Edycja partnera";

            partnerId = id;

            LoadData();
        }

        /// <summary>
        /// Konstruktor wykorzystywany przy tworzeniu nowego partnera.
        /// </summary>
        /// <param name="mainWindow"></param>
        public PartnerDialog(MainWindow mainWindow) : base(mainWindow)
        {
            this.mainWindow = mainWindow;

            InitializeComponent();
            this.DataContext = new WarehouseValidationRule();

            Header.Content = "Wprowadź dane:";
            Title = "Tworzenie nowego partnera";
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
            WarehouseValidationRule rule = DataContext as WarehouseValidationRule;

            rule.Name = NameTB.Text = partner.Warehouse.Name;
            rule.City = CityTB.Text = partner.City;
            rule.Code = CodeTB.Text = partner.Code;
            rule.Street = StreetTB.Text = partner.Street;
            rule.Number = NumberTB.Text = partner.Num;
            rule.Phone = PhoneTB.Text = partner.Tel;
            MailTB.Text = partner.Mail;
        }

        /// <summary>
        /// Zapis danych i zamknięcie okna
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveClick(object sender, RoutedEventArgs e)
        {
            (sender as Button).IsEnabled = false;

            PartnerDto data = new PartnerDto()
            {
                Name = NameTB.Text,
                City = CityTB.Text,
                Code = CodeTB.Text,
                Street = StreetTB.Text,
                Num = NumberTB.Text,
                Tel = PhoneTB.Text,
                Mail = MailTB.Text,
            };

            if (partnerId == -1)
            {
                WarehouseInfoDto w = new WarehouseInfoDto()
                {
                    Name = data.Name,
                    City = data.City,
                    Code = data.Code,
                    Street = data.Street,
                    Num = data.Num,
                    Tel = data.Tel,
                    Mail = data.Mail,
                    Deleted = false,
                    Internal = false
                };

                data.Warehouse = w;

                Execute(() => PartnersService.AddNew(new Request<PartnerDto>(data)), t =>
                    {
                        SectorDto s = new SectorDto()
                        {
                            Deleted = false,
                            Limit = 0,
                            Number = 1,
                            WarehouseId = t.Data.Warehouse.Id,
                        };

                        Execute(() => WarehousesService.AddSector(new Request<SectorDto>(s)), x =>
                            {
                                mainWindow.ReloadWindow();
                                this.Close();
                            }, x =>
                            {
                                DefaultExceptionHandler(x);
                                mainWindow.ReloadWindow();
                                this.Close();
                            });
                    }, t =>
                    {
                        DefaultExceptionHandler(t);
                        mainWindow.ReloadWindow();
                        this.Close();
                    });
            }
            else
            {
                partner.Warehouse.City = partner.City = CityTB.Text;
                partner.Warehouse.Code = partner.Code = CodeTB.Text;
                partner.Warehouse.Mail = partner.Mail = MailTB.Text;
                partner.Warehouse.Name = partner.Name = NameTB.Text;
                partner.Warehouse.Num = partner.Num = NumberTB.Text;
                partner.Warehouse.Street = partner.Street = StreetTB.Text;
                partner.Warehouse.Tel = partner.Tel = PhoneTB.Text;

                Execute(() => PartnersService.Update(new Request<PartnerDto>(partner)), t =>
                    {
                        //Execute(() => WarehousesService.Edit(new Request<WarehouseInfoDto>(partner.Warehouse)), x =>
                        //    {
                                mainWindow.ReloadWindow();
                                this.Close();
                            //}, x =>
                            //    {
                            //        DefaultExceptionHandler(x);
                            //        mainWindow.ReloadWindow();
                            //        this.Close();
                            //    });
                        
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
