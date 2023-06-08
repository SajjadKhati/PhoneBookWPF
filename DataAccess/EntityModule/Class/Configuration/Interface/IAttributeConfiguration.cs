using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityModule.Class.Configuration.Interface
{
    /// <summary>
    ///  پیکربندی صفت جدول در دیتابیس را انجام میدهد .<br/>
    /// این ، یک اینترفیس پایه هست و شامل اعضایی برای پیکربندی صفت جدول در دیتابیس هست که برای همه ی کلاس های موجودیت ها باید بکار برده شوند . <br/><br/>
    /// 
    /// هر چند ممکن هست که در نسخه های بعدی ، موجودیتی وجود داشته باشد که نیاز به تغییر نوع داده ای برای صفت اش و یا حتی نیاز به تغییر not null بودنِ صفت اش نباشد
    /// و بنابراین نیاز به این نباشد که این اینترفیس ، از اینترفیس های IAttributeDataTypeConfiguration و IAttributeRequireConfiguration مشتق شوند ،
    /// اما چون ، هم تمام موجودیت هایی که در این نسخه از نرم افزار هستند ، به این قابلیت ها نیاز دارند و هم اینکه حداقل فعلا ، قصد نداریم که نسخه ی بعدی این نرم افزار را
    /// بسازیم ، بنابراین این اینترفیس را از اینترفیس های نامبرده شده ، مشتق کردیم .<br/><br/>
    /// 
    /// برای اجرای اصل interface segregation در solid ، این اینترفیس که برای پیاده سازی کاری مجزاست را جدا کردیم .
    /// </summary>
    internal interface IAttributeConfigurationAggregate  : IAttributeDataTypeConfiguration, IAttributeRequireConfiguration { }
}
