using Model.PhoneBookModule.Class;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ViewModel.CollectionViewOperationsModule.Class
{
    internal class FirstProvinceGroupingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IList<Address> groupingFirstProvinceAddresses = value as IList<Address>;
            if (groupingFirstProvinceAddresses == null || groupingFirstProvinceAddresses.Count < 1 ||
                groupingFirstProvinceAddresses[0].Province == null)
                return null;

            return groupingFirstProvinceAddresses[0].Province;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }


    }
}
