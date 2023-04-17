using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityModule.Class.Configuration.Interface
{
    /// <summary>
    ///  پیکربندی ضروری بودن یک صفت جدول در دیتابیس را انجام میدهد .<br/><br/>
    /// 
    /// برای اجرای اصل interface segregation در solid ، این اینترفیس که برای پیاده سازی کاری مجزاست را جدا کردیم .
    /// </summary>
    internal interface IAttributeRequireConfiguration
    {
        /// <summary>
        /// ضروری بودن یا همان قابلیت Not-Null را برای صفت جدول در دیتابیس پیکربندی میکند .
        /// </summary>
        void SetAttributeRequire();
    }
}
