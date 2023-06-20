using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PhoneBook.ViewBindingConverter
{
    internal class DataGridRowLiningUpConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                int dataGridRowAlternationIndex = System.Convert.ToInt32(value);
                dataGridRowAlternationIndex++;
                return dataGridRowAlternationIndex;
            }
            catch (Exception exception)
            {
                return Binding.DoNothing;
            }
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }


    }
}
