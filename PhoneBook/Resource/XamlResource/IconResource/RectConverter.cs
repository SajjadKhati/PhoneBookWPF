using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace PhoneBook.Resource.XamlResource.IconResource
{
    internal class RectConverter : IMultiValueConverter
    {
        //public object Convert(object values, Type targetType, object parameter, CultureInfo culture)
        //{
        //    //if (values.Length == 2 && values[0] is double width && values[1] is double height)
        //    //{
        //    //    return new Rect(1, 1, width - 2, height - 2);
        //    //}

        //    return values;
        //}


        //public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        //{
        //    throw new NotImplementedException();
        //}


        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (values.Length == 2 && values[0] is double width && values[1] is double height)
                {
                    return new Rect(1, 1, width - 2, height - 2);
                }
            }
            catch (Exception exception)
            {
            }

            return Binding.DoNothing;
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }


    }

}
