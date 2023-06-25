using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace PhoneBook.VisualElementModule
{
    internal static class VisualParentFinder
    {
        internal static ResultParentType GetVisualParent<ResultParentType>(FrameworkElement searchStartChild, 
            string parentControlName) where ResultParentType : FrameworkElement
        {
            FrameworkElement foundedParent = VisualTreeHelper.GetParent(searchStartChild) as FrameworkElement;
            if (foundedParent == null)
                return null;

            /// برای اینکه اگر نوعِ foundedParent از نوعِ FrameworkElement بود ، مقدار null در آن متغییر ذخیره نشود ، مستقیما به نوعِ ResultParentType تبدیل نمیکنیم .
            /// بلکه مجددا مقدار آنرا تبدیل میکنیم .
            ResultParentType convertedFoundedParent = foundedParent as ResultParentType;
            if (convertedFoundedParent != null && convertedFoundedParent.Name == parentControlName)
                return convertedFoundedParent;
            else
            {
                ResultParentType foundedAncestor = GetVisualParent<ResultParentType>(foundedParent, parentControlName);
                if (foundedAncestor != null && foundedAncestor.Name == parentControlName)
                    return foundedAncestor;
            }

            return null;
        }
    }
}
