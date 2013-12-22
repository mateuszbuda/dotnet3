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
using WMS.Client.Misc;

namespace WMS.Client.Dialogs
{
    /// <summary>
    /// Klasa pomocnicza reprezentująca produkt w grupie
    /// </summary>
    public partial class ProductGroupRow : UserControl
    {
        ListView parent;

        public ProductGroupRow(ListView p)
        {
            InitializeComponent();
            this.DataContext = new QuantityValidationRule();
            parent = p;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (parent != null)
                parent.Items.Remove(this);
        }
    }
}
