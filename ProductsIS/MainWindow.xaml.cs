using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    

    class Page : INotifyPropertyChanged
    {
        public int NumberPage { get; set; }
        private bool _isChecked;
        public string Title
        {
            get
            {
                return NumberPage.ToString();
            }
        }
        public List<Product> RangeProducts { get; set; }
        public bool Check
        {
            get=> _isChecked;
            set
            {
                _isChecked = value;
                OpPropetyChange("Check");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OpPropetyChange(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
    class FilterProductType : INotifyPropertyChanged
    {
        private ProductType _productType;

        public event PropertyChangedEventHandler PropertyChanged;

        public ProductType ProductType
        {
            get
            {
                return _productType;
            }
            set
            {
                _productType = value;
                Title = _productType.Title;
            }
        }
        private bool _check = false;
        public bool Check
        {
            get
            {
                return _check;
            }
            set
            {
                _check = value;
                OnPropertyChanged("Check");
            }
        }
        public string Title { get; private set; } = "Все типы";
        public void OnPropertyChanged(string str)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(str));
        }
    }
    public partial class MainWindow : Window
    {

        int currentPage;
        int countPage;
        int countElements;
        List<Page> pagesList = new List<Page>();
        public SortElement SortEl { get; set; }
        FilterProductType productTypeAll;
        List<SortElement> sortElements;
        List<FilterProductType> filterProductType = new List<FilterProductType>();
        public MainWindow()
        {
            InitializeComponent();
            productTypeAll = new FilterProductType();
            sortElements = new List<SortElement>()
            {
                new SortElement(){Id = 1, Name = "Наименование"},
                new SortElement(){Id = 2, Name = "Номер цеха"},
                new SortElement(){Id = 3, Name = "Минимальная стоимость"}
            };
            sortComboBox.ItemsSource = sortElements;
            filterProductType.Add(productTypeAll);
            filterProductType.AddRange(ConnectDataBAse.GetContext().ProductType.Select(p=>new FilterProductType() { ProductType = p}));
            filterComboBox.ItemsSource = filterProductType;
            UpdateList();
        }
        public void UpdateList()
        {
            pagesList.Clear();
            ICollection<Product> products = ConnectDataBAse.GetContext().Product.ToList();
            if (!String.IsNullOrEmpty(searchTextBox.Text.Trim()))
                products = products.Where(p => p.Title.Contains(searchTextBox.Text) || (String.IsNullOrEmpty(p.Description) && p.Description.Contains(searchTextBox.Text))).ToList();
            List<ProductType> filterList = filterProductType.GetRange(1,filterProductType.Count-1).Where(p => p.Check == true).Select(p => p.ProductType).ToList();
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
                numberNavigationPage.ItemsSource = null;
                navigationStackPanel.Visibility = Visibility.Visible;
                countPage = countElements / 20;
                int i;
                for (i = 0; i < countPage; i++)
                    pagesList.Add(new Page() { NumberPage = i + 1, RangeProducts = products.ToList().GetRange(i * 20, 20) });
                int remainsProduct = countElements % 20;
                if(remainsProduct>0)
                {
                    pagesList.Add(new Page() { NumberPage = i + 1, RangeProducts = products.ToList().GetRange(i * 20, remainsProduct) });
                    countPage++;
                }
                currentPage = 1;
                numberNavigationPage.ItemsSource = pagesList;
                pagesList[0].Check = true;
                listBox.ItemsSource = pagesList[0].RangeProducts;
            }
            else
            {
                navigationStackPanel.Visibility = Visibility.Hidden;
                listBox.ItemsSource = products;
            }

        }
        private void Btn_Back_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage != 1)
                pagesList[currentPage - 2].Check = true;
        }

        private void nextBtn_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage != countPage)
                pagesList[currentPage].Check = true;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (((FilterProductType)checkBox.DataContext) != productTypeAll)
                productTypeAll.Check = false;
            else filterProductType.GetRange(1, filterProductType.Count - 1).ForEach(p => p.Check = false);
        }
        private void filterComboBox_DropDownClosed(object sender, EventArgs e)=>UpdateList();
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)=>UpdateList();
        private void filterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => ((ComboBox)sender).SelectedIndex = -1;

        private void sortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            SortWindow sortWindow = new SortWindow((SortElement)comboBox.SelectedItem);
            sortWindow.Owner = this;
            sortWindow.Show();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            Page page = (sender as RadioButton).DataContext as Page;
            currentPage = page.NumberPage;
            listBox.ItemsSource = page.RangeProducts;
        }
    }
}
