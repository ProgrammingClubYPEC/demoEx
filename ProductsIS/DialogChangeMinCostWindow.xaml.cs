using System;
using System.Collections;
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

namespace ProductsIS
{
    /// <summary>
    /// Логика взаимодействия для DialogChangeMinCostWindow.xaml
    /// </summary>
    public partial class DialogChangeMinCostWindow : Window
    {
        public decimal Value { get;set; }
        private IList<Product> products;
        public DialogChangeMinCostWindow(IList<Product> products)
        {
            InitializeComponent();
            this.DataContext = this;
            this.products = products;
            valueTextBox.DataContext = this;
            Value = Math.Round(products.Average(p => p.MinCostForAgent),2);
        }
        private void isActiveButton_Click(object sender, RoutedEventArgs e) => this.DialogResult = true;
    }
}
