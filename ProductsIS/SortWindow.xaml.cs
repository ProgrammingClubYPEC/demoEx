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

namespace ProductsIS
{
    /// <summary>
    /// Логика взаимодействия для SortWindow.xaml
    /// </summary>
    public partial class SortWindow : Window
    {
        private SortElement element;
        public SortWindow(SortElement sortElement)
        {
            InitializeComponent();
            element = sortElement;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (upRadioButton.IsChecked == true)
            {
                element.Direction = true;
                this.Close();
            }
            else if (downRadioButton.IsChecked == true)
            {
                element.Direction = false;
                this.Close();
            }
            else MessageBox.Show("Выбирете метод сортировки");


        }

        private void Window_Closed(object sender, EventArgs e)
        {
            ((MainWindow)this.Owner).SortEl = element;
            ((MainWindow)this.Owner).UpdateList();
        }
    }
}
