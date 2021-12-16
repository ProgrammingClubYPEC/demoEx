using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
    /// Логика взаимодействия для EditProductWindow.xaml
    /// </summary>
    public partial class EditProductWindow : Window
    {
        public Product EditProduct { get; set; }
        private ImageHelp ImageHelP { get; set; }
        bool isAdd = false;
        class ImageHelp:INotifyPropertyChanged
        {
            public Product Product { get; set; }
            public string Image
            {
                get => Product.Image;
                set
                {
                    Product.Image = value;
                    OnPropertyChanged("Image");
                }
            }
            private void OnPropertyChanged(string property) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
            public event PropertyChangedEventHandler PropertyChanged;
        }
        public EditProductWindow(Product product)
        {
            InitializeComponent();
            EditProduct = product;
            typeProduct.ItemsSource = ConnectDataBAse.GetContext().ProductType.ToList();
            UpdateMaterialList();
            ImageHelP = new ImageHelp() { Product = product };
            DataContext = EditProduct;
            image.DataContext = ImageHelP;
        }
        public EditProductWindow()
        {
            InitializeComponent();
            EditProduct = new Product();
            typeProduct.ItemsSource = ConnectDataBAse.GetContext().ProductType.ToList();
            UpdateMaterialList();
            ImageHelP = new ImageHelp() { Product = EditProduct };
            DataContext = EditProduct;
            isAdd = true;
            saveChangeButton.Content = "Добавить товар";
            image.DataContext = ImageHelP;
        }
        private void UpdateMaterialList()
        {
            materialComboBox.ItemsSource = ConnectDataBAse.GetContext().Material.ToList().Except(EditProduct.ProductMaterial.Select(p => p.Material)).ToList();
            materialsListView.ItemsSource = null;
            materialsListView.ItemsSource = EditProduct.ProductMaterial;
        }
        private void Add_Material_Button_Click(object sender, RoutedEventArgs e)
        {
            Material material = materialComboBox.SelectedItem as Material;
            if (material != null)
            {
                int count;
                if (int.TryParse(countMaterialTextBoxt.Text, out count))
                {
                    ConnectDataBAse.GetContext().ProductMaterial.Add(new ProductMaterial() { Material = material, Product = EditProduct, Count = count });
                    UpdateMaterialList();
                    countMaterialTextBoxt.Text = null;
                }
            }
        }
        private void Remove_Material_Button_Click(object sender, RoutedEventArgs e)
        {
            if(materialsListView.SelectedItem != null)
            {
                EditProduct.ProductMaterial.Remove(materialsListView.SelectedItem as ProductMaterial);
                UpdateMaterialList();
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files| *.jpg; *.jpeg; *.png;";
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string path = "products/" + DateTime.Now.ToBinary().ToString() + System.IO.Path.GetExtension(openFileDialog.FileName);
                    File.Copy(openFileDialog.FileName, System.IO.Path.GetFullPath(path));
                    MessageBox.Show(path);
                    ImageHelP.Image = "/"+path;
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(isAdd)
            {
                //Тут надо сделать проверки на заполняемость полей
                ConnectDataBAse.GetContext().Product.Add(EditProduct);
            }
            ConnectDataBAse.GetContext().SaveChanges();
            ConnectDataBAse.ApplyDataBaseChange();
            (this.Owner as MainWindow).UpdateList();
            this.Close();
        }
    }
}
