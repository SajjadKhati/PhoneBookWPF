using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityModule.Class.Configuration.Interface
{
    /// <summary>
    ///  پیکربندی جدول دیتابیس را انجام میدهد .<br/><br/>
    /// 
    /// برای اجرای اصل interface segregation در solid ، این اینترفیس که برای پیاده سازی کاری مجزاست را جدا کردیم .
    /// </summary>
    internal interface ITableConfiguration
    {
        /// <summary>
        /// نام جدول را در دیتابیس پیکربندی میکند .
        /// </summary>
        void SetTableName();
    }
}
