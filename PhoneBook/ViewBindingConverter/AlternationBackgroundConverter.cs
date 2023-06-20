using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace PhoneBook.ViewBindingConverter
{
    internal class AlternationBackgroundConverter : IValueConverter
    {
        internal Brush BackgroundBrush { get; set; }


        internal Brush AlternationBackgroundBrush { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (this.BackgroundBrush == null || this.AlternationBackgroundBrush == null)
                return Binding.DoNothing;

            try
            {
                int alternationIndex = System.Convert.ToInt32(value);
                if (alternationIndex % 2 == 0)
                    return this.BackgroundBrush;
                else
                    return this.AlternationBackgroundBrush;
            }
            catch (Exception e) { }

            return Binding.DoNothing;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }


    }
}
