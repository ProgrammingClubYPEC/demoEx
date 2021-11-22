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
    class TextBlockClick : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public TextBlockClick()
        {

        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            MessageBox.Show("Тест");
        }
    }
    class Page
    {
        ICommand command = new TextBlockClick();
        public ICommand TextBlockClickCommand
        {
            get => command;
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
        public SortElement SortEl { get; set; }
        ICollection<Product> products;
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
            testBlock.DataContext = new Page();
            filterProductType.Add(productTypeAll);
            filterProductType.AddRange(ConnectDataBAse.GetContext().ProductType.Select(p=>new FilterProductType() { ProductType = p}));
            filterComboBox.ItemsSource = filterProductType;
            UpdateList();
        }
        public void UpdateList()
        {
            wrapPanel.Children.Clear();
            products = ConnectDataBAse.GetContext().Product.ToList();
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
        public void UpdatePage()=>listBox.ItemsSource = products.ToList().GetRange((currentPage-1)*20,20);
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

    }
}
