using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ProductsIS
{
    class ListToStrMaterial : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str = null;
            ICollection<ProductMaterial> productMaterials = value as ICollection<ProductMaterial>;
            foreach(ProductMaterial productMaterial in productMaterials)
            {
                str += productMaterial.Material.Title + ", ";
            }
            return str?.Remove(str.Length-2,2);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
