using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using ViewModel.CollectionViewOperationsModule.Enum;
using ViewModel.CollectionViewOperationsModule.Interface;

namespace ViewModel.CollectionViewOperationsModule.Class
{
    public class PersonGroupingPropertyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string personGroupingPropertyName = value as string;
            if (personGroupingPropertyName == null)
                return Binding.DoNothing;

            switch (personGroupingPropertyName)
            {
                case "هیچ کدام":
                    return PersonGroupingProperties.None;
                case "نام":
                    return PersonGroupingProperties.FirstName;
                case "نام خانوادگی":
                    return PersonGroupingProperties.LastName;
                case "اولین استان":
                    return PersonGroupingProperties.FirstProvince;
                case "اولین شهر":
                    return PersonGroupingProperties.FirstCity;
            }
            return Binding.DoNothing;
        }


    }
}
