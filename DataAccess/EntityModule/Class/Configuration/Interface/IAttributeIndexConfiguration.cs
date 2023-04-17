using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityModule.Class.Configuration.Interface
{
    /// <summary>
    ///  پیکربندی Index گذاشتن برای صفت جدول در دیتابیس را انجام میدهد .<br/><br/>
    ///
    /// این اینترفیس ، توسط اینترفیس های پایه ، مشتق نمیشود . چون ممکن هست که فقط توسط بعضی از کلاس ها پیاده سازی شود و در همه ی کلاس ها ، یاز نیست .<br/>
    /// برای اجرای اصل interface segregation در solid ، این اینترفیس که برای پیاده سازی کاری مجزاست را جدا کردیم .
    /// </summary>
    internal interface IAttributeIndexConfiguration
    {
        /// <summary>
        /// Index را برای صفت جدول در دیتابیس پیکربندی میکند .
        /// </summary>
        void SetAttributeIndex();
    }
}
