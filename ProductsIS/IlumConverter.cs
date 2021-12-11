using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ProductsIS
{
    class IlumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isSold = true;
            ICollection<ProductSale> productSales = value as ICollection<ProductSale>;
            foreach(ProductSale productSale in productSales.ToList())
            {
                DateTime saleDate = productSale.SaleDate.AddMonths(1);
                if(saleDate.Month == DateTime.Now.Month && saleDate.Year == DateTime.Now.Year)
                {
                    isSold = false;
                    break;
                }
            }
            return isSold ? new SolidColorBrush(Colors.IndianRed) : new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
