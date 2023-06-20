using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace ToolTipsValueConverter
{
    class ToolTipsRecBoundMultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double toolTipWidth = System.Convert.ToDouble(values[0]);
            double toolTipHeight = System.Convert.ToDouble(values[1]);
            // چون ویژال استودیو 2022 در xaml designer گیر میده که width و height باید اعداد مثبت باشن ، براشون متد قدر مطلق را توسط متدِ Math.Abs در نظر گرفتیم .
            double rectangleWidth = Math.Abs(toolTipWidth - 2);
            double pathHeight = toolTipHeight - 2;
            // برای خروجیِ double گرفتن ، حداقل ، یک طرفِ معادله باید از نوع اعشاری باشه . واسه ی همین عدد 20 در پایین را از نوع double گرفتیم .
            // چون ویژال استودیو 2022 در xaml designer گیر میده که width و height باید اعداد مثبت باشن ، براشون متد قدر مطلق را توسط متدِ Math.Abs در نظر گرفتیم .
            double rectangleHeight = Math.Abs(pathHeight * (80d / 100));
            double rectangleY = pathHeight - rectangleHeight;
            return new Rect(1, rectangleY, rectangleWidth, rectangleHeight);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }




    class ToolTipsTrianglePointValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double triangleX = System.Convert.ToDouble(parameter);
            double toolTipHeight = System.Convert.ToDouble(value);
            double pathHeight = toolTipHeight - 2;
            double triangleY = pathHeight * (25d / 100);
            return new Point(triangleX, triangleY); 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }




    class ToolTipsPlacementValueConverter : IValueConverter
    {
        /// <summary>
        /// مقداری از نوع PlacementMode در شیِ ToolTip را میگیره و اگه این مقدار ، با یکی از مقادیر PlacementMode.Right یا PlacementMode.Left یا -
        /// PlacementMode.Center یا PlacementMode.MousePoint برابر بود ، مقدار true را برمیگردونه 
        /// تا در DataTrigger ئه مورد نظر ، حالت بالون برای ToolTip ئه مورد نظر ، مخفی بشه و حالتِ Border فقط داشته باشه وگرنه مقدار false را برمیگردونه-
        /// تا ToolTip مون ، حالت عادی که شکل بالون هست را داشته باشه .
        /// </summary>
        /// <param name="value">
        /// مقداری از نوع اینامِ PlacementMode در شیِ ToolTip</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>
        /// مقدار بولین را برمیگردونه .
        /// در بالا توضیح داده شد .
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            PlacementMode toolTipPlacementMode = (PlacementMode)value;
            PlacementMode[] checkPlacementModes = new PlacementMode[] { PlacementMode.Right, PlacementMode.Left, PlacementMode.Center, PlacementMode.MousePoint };
            foreach (PlacementMode checkPlacementMode in checkPlacementModes)
            {
                if (toolTipPlacementMode == checkPlacementMode)
                    return true;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
