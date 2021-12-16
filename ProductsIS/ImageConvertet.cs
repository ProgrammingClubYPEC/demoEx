using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ProductsIS
{
    class ImageConvertet : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null) return "picture.png";
            try
            {
                string path = Path.GetFullPath(((string)value).Replace(',', '.').Remove(0,1));
                return File.Exists(path) ? path : "picture.png";
            }
            catch(Exception)
            {
                return "picture.png";
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
