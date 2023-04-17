using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityModule.Class.Configuration.Interface
{
    /// <summary>
    ///  پیکربندی نوع داده ی صفت جدول در دیتابیس را انجام میدهد .<br/><br/>
    /// 
    /// برای اجرای اصل interface segregation در solid ، این اینترفیس که برای پیاده سازی کاری مجزاست را جدا کردیم .
    /// </summary>
    internal interface IAttributeDataTypeConfiguration
    {
        /// <summary>
        /// نوع داده ی صفت را در دیتابیس پیکربندی میکند .
        /// </summary>
        void SetAttributeDataType();
    }
}
