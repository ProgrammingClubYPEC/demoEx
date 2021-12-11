using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ProductsIS
{
    class DecimalValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            decimal v = 0;
            try
            {
                if (value.ToString().Length > 0)
                    v = decimal.Parse(value.ToString(), CultureInfo.InvariantCulture);
                else return new ValidationResult(false, "Пустое поле");
                
            }
            catch(Exception)
            {
                return new ValidationResult(false, "Неверный формат данных");
            }
            if (v < 0) return new ValidationResult(false, "Введено отрицательное значение");
            if (NumberOfDecimalPlaces(v) > 2) return new ValidationResult(false, "Разрешены только сотые части");
            return ValidationResult.ValidResult;
        }
        private int NumberOfDecimalPlaces(decimal value)
        {
            String[] stringsSplit = value.ToString().Split(',');
            return stringsSplit.Length == 1 ? 0 : stringsSplit[1].Length;
        }
    }
}
