using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ProductsIS
{
    class SummCostMaterialConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal summ = 0;
            ICollection<ProductMaterial> productMaterials = value as ICollection<ProductMaterial>;
            foreach (ProductMaterial productMaterial in productMaterials)
            {
                summ += productMaterial.Material.Cost;
            }
            return summ;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
