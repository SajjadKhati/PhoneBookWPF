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
    internal class FirstCityGroupingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IList<Address> groupingFirstCityAddresses = value as IList<Address>;
            if (groupingFirstCityAddresses == null || groupingFirstCityAddresses.Count < 1 || groupingFirstCityAddresses[0].City == null)
                return null;

            return groupingFirstCityAddresses[0].City;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }


    }
}
