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

namespace ProductsIS
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        int currentPage;
        int countPage;
        int countElements;
        public SortElement SortEl { get; set; }
        ICollection<Product> products;
        ProductType productTypeAll;
        List<ProductType> filterList = new List<ProductType>();
        List<SortElement> sortElements;
        public MainWindow()
        {
            InitializeComponent();
            productTypeAll = new ProductType();
            productTypeAll.Title = "Все типы";
            sortElements = new List<SortElement>()
            {
                new SortElement(){Id = 1, Name = "Наименование"},
                new SortElement(){Id = 2, Name = "Номер цеха"},
                new SortElement(){Id = 3, Name = "Минимальная стоимость"}
            };
            sortComboBox.ItemsSource = sortElements;
            List<ProductType> filterProductType = new List<ProductType>();
            filterProductType.Add(productTypeAll);
            filterProductType.AddRange(ConnectDataBAse.GetContext().ProductType.ToList());
            filterComboBox.ItemsSource = filterProductType;
            
            UpdateList();
        }
        public void UpdateList()
        {
            wrapPanel.Children.Clear();
            products = ConnectDataBAse.GetContext().Product.ToList();
            if (!String.IsNullOrEmpty(searchTextBox.Text.Trim()))
                products = products.Where(p => p.Title.Contains(searchTextBox.Text) || (String.IsNullOrEmpty(p.Description) && p.Description.Contains(searchTextBox.Text))).ToList();
            if (filterList.Count > 0)
                products = products.Where(p => filterList.Contains(p.ProductType)).ToList();
            if (SortEl != null)
            {
                switch (SortEl.Id)
                {
                    case 1:
                        if (SortEl.Direction)
                            products = products.OrderBy(p => p.Title).ToList();
                        else products = products.OrderByDescending(p => p.Title).ToList();
                            break;
                    case 2:
                        if (SortEl.Direction)
                            products = products.OrderBy(p => p.ProductionWorkshopNumber).ToList();
                        else products = products.OrderByDescending(p => p.ProductionWorkshopNumber).ToList();
                        break;
                    case 3:
                        if (SortEl.Direction)
                            products = products.OrderBy(p => p.MinCostForAgent).ToList();
                        else products = products.OrderByDescending(p => p.MinCostForAgent).ToList();
                        break;
                }
            }
            countElements = products.Count;
            if (countElements > 20)
            {
                navigationStackPanel.Visibility = Visibility.Visible;
                countPage = countElements / 20;
                for (int i = 0; i < countPage; i++)
                {
                    Button btn = new Button();
                    btn.Content = (i + 1).ToString();
                    btn.Tag = i + 1;
                    btn.Click += Btn_Click;
                    wrapPanel.Children.Add(btn);
                }
                currentPage = 1;
                UpdatePage();
            }
            else
            {
                navigationStackPanel.Visibility = Visibility.Hidden;
                listBox.ItemsSource = products;
            }

        }
        public void UpdatePage()
        {
            int index = 0;
            Product[] pr = new Product[20];
            if (currentPage != 1) 
                index = ((currentPage - 1) * 20) - 1;
            products.ToList().CopyTo(index, pr, 0, 20);
            listBox.ItemsSource = pr;
        }
        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            currentPage = (int)(sender as Button).Tag;
            UpdatePage();
        }

        private void Btn_Back_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage != 1)
            {
                currentPage -= 1;
                UpdatePage();
            }
        }

        private void nextBtn_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage != countPage)
            {
                currentPage += 1;
                UpdatePage();
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (((ProductType)checkBox.DataContext) != productTypeAll)
                filterList.Add((ProductType)checkBox.DataContext);
            else
            {
                filterList.Clear();
                filterComboBox.ItemsSource = null;
                List<ProductType> filterProductType = new List<ProductType>();
                filterProductType.Add(productTypeAll);
                filterProductType.AddRange(ConnectDataBAse.GetContext().ProductType.ToList());
                filterComboBox.ItemsSource = filterProductType;
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            filterList.Remove((ProductType)checkBox.DataContext);
        }

        private void filterComboBox_DropDownClosed(object sender, EventArgs e)
        {
            UpdateList();

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateList();
        }

        private void sortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            SortWindow sortWindow = new SortWindow((SortElement)comboBox.SelectedItem);
            sortWindow.Owner = this;
            sortWindow.Show();
        }
    }
}
