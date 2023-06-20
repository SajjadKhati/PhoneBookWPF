using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PhoneBook.ViewBindingConverter
{
    internal class AddressItemNumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int addressItemNumber = 0;
            try
            {
                addressItemNumber = System.Convert.ToInt32(value);
                addressItemNumber++;
            }
            catch (Exception e)
            {
                return Binding.DoNothing;
            }
            return addressItemNumber;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


    }
}
