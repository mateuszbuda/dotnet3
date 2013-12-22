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
using System.Windows.Shapes;
using WMS.ServicesInterface.DTOs;
using WMS.ServicesInterface.DataContracts;
using WMS.Client.Dialogs;
using WMS.ServicesInterface;

namespace WMS.Client.Dialogs
{
    /// <summary>
    /// Interaction logic for UserDialog.xaml
    /// </summary>
    public partial class UserDialog : BaseDialog
    {
        MainWindow mainWindow;

        public UserDialog(MainWindow mainWindow)
            : base(mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (username.Text == null || username.Text.Length < 3 || username.Text.Length > 30 ||
                password.Password == null || password.Password.Length < 3 ||
                permissions.SelectedIndex < 0)
            {
                MessageBox.Show("Wprowadzono niepoprawne dane.", "Błąd!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            UserDto user = new UserDto()
            {
                Username = username.Text,
                Password = password.Password,
                PermissionsVal = permissions.SelectedIndex,
                Permissions = (PermissionLevel)permissions.SelectedIndex,
            };

            Execute(() => AuthenticationService.AddNew(new Request<UserDto>(user)), t =>
                {
                    mainWindow.ReloadWindow();
                    this.Close();
                },
                t =>
                {
                    DefaultExceptionHandler(t);
                    mainWindow.ReloadWindow();
                    this.Close();
                });
        }
    }
}
