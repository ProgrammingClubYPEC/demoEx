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
    enum SortType
    {
        Name,
        NumberWorkShop,
        MinCost
    }
    class SortProductType : INotifyPropertyChanged
    {
        public string SortName { get; set; }
        private SortType _sortType;
        public SortType SortType
        {
            get =>_sortType;
            set
            {
                switch(value)
                {
                    case SortType.MinCost:
                        SortName = "Мин.стоимость";
                        break;
                    case SortType.Name:
                        SortName = "Наименование";
                        break;
                    case SortType.NumberWorkShop:
                        SortName = "Номер цеха";
                        break;
                }
                _sortType = value;
            }
        }
        private bool? _condition;
        public bool? Сondition
        {
            get => _condition;
            set
            {
                _condition = value;
                OnPropertyChange("Сondition");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChange(string property) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
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
        List<SortProductType> sortElements;
        List<FilterProductType> filterProductType = new List<FilterProductType>();
        public MainWindow()
        {
            InitializeComponent();
            productTypeAll = new FilterProductType();
            sortElements = new List<SortProductType>()
            {
                new SortProductType(){SortType = SortType.MinCost},
                new SortProductType(){SortType = SortType.Name},
                new SortProductType(){SortType = SortType.NumberWorkShop}
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
            foreach(SortProductType sortProductType in sortElements)
            {
                if(sortProductType.Сondition !=null)
                {
                    if(sortProductType.Сondition == false)
                    {
                        switch(sortProductType.SortType)
                        {
                            case SortType.Name:
                                products = products.OrderBy(p => p.Title).ToList();
                                break;
                            case SortType.MinCost:
                                products = products.OrderBy(p => p.MinCostForAgent).ToList();
                                break;
                            case SortType.NumberWorkShop:
                                products = products.OrderBy(p => p.ProductionWorkshopNumber).ToList();
                                break;
                        }
                    }
                    else
                    {
                        switch (sortProductType.SortType)
                        {
                            case SortType.Name:
                                products = products.OrderByDescending(p => p.Title).ToList();
                                break;
                            case SortType.MinCost:
                                products = products.OrderByDescending(p => p.MinCostForAgent).ToList();
                                break;
                            case SortType.NumberWorkShop:
                                products = products.OrderByDescending(p => p.ProductionWorkshopNumber).ToList();
                                break;
                        }
                    }
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

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            Page page = (sender as RadioButton).DataContext as Page;
            currentPage = page.NumberPage;
            listBox.ItemsSource = page.RangeProducts;
        }

        private void changeMinCostButton_Click(object sender, RoutedEventArgs e)
        {
            IList<Product> selectionProductList = listBox.SelectedItems.Cast<Product>().ToList();
            DialogChangeMinCostWindow dialogChangeMinCostWindow = new DialogChangeMinCostWindow(selectionProductList);
            if(dialogChangeMinCostWindow.ShowDialog()==true)
            {
                selectionProductList.ToList().ForEach(p => p.MinCostForAgent = dialogChangeMinCostWindow.Value);
                ConnectDataBAse.GetContext().SaveChanges();
                ConnectDataBAse.ApplyDataBaseChange();
                listBox.SelectedItems.Clear();
            }
        }
        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => changeMinCostButton.Visibility = (sender as ListBox)?.SelectedItems.Count > 0 ? Visibility.Visible : Visibility.Hidden;
    }
}
