using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PoshtibangirTolo.View.CustomControl
{
    public class HasValueConverter : IValueConverter
    {
        /// <summary>
        /// متد پیاده سازی شده از اینترفیس IValueConverter برای تبدیل _ تبدیل Source Type یا Target Type و ... به هر نوع دلخواه
        /// برای اینکه بدونیم پروپرتی هایی که با نام ButtonShape ختم میشن _ و در این کلاس وجود دارن و از نوع Path هستن _ ، چک کنیم که در Template ها مقدار دارن یا نه ، از تبدیل کننده ها استفاده میکنیم
        /// </summary>
        /// <param name="value">مقدار Source Type که همون مقدار پروپرتی ای هست که در زمان Binding اش از Converter استفاده کرده بودیم . در این مثال ، برای پروپرتی هایی که با نام ButtonShape ختم میشن ، از Converter استفاده میکنیم . بنابراین پارامتر value همون مقادیرِ این پروپرتی که از نوع Path هستند ، خواهند بود .</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>مقدار بازگشتی در اینجا ، نوع bool هست که مشخص میکنه که مقدار پارامتر value _ همون مقدار پروپرتی هایی که با ButtonShape ختم میشن _ مقدار داره یا نه . تا در صورت مقدار نداشتن این پروپرتی ها ، در Template ها ، رسم ای برای اونها انجام نشه .</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // مقدار بازگشتی در اینجا ، نوع bool هست که مشخص میکنه که مقدار پارامتر value _ همون مقدار پروپرتی هایی که با ButtonShape ختم میشن _ مقدار داره یا نه . تا در صورت مقدار نداشتن این پروپرتی ها ، در Template ها ، رسم ای برای اونها انجام نشه .
            return value != null;
        }


        /// <summary>
        /// برعکس متد Convert هست . در اینجا ، به این متد نیازی نداریم .
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }


    }
}
