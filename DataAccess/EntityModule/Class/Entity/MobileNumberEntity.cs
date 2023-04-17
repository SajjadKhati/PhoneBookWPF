using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityModule.Class.Entity
{
    /// <summary>
    /// موجودیت شماره موبایل<br/><br/>
    /// 
    /// این موجودیت ، رابطه ی "چند به یک" را موجودیتِ
    /// <see cref="T:DataAccess.EntityModule.Class.Entity.PersonEntity" />
    /// دارد . یعنی هر شخص ، میتواند چندین رکورد از موجودیت شماره موبایل را داشته باشد .
    /// </summary>
    public class MobileNumberEntity
    {
        /// <summary>
        /// شناسه ی موجودیت شماره موبایل
        /// </summary>
        public int Id{ get; set; }


        /// <summary>
        ///  شماره موبایل
        /// </summary>
        public string MobileNumber{ get; set; }


        /// <summary>
        /// Navigation Property برای موجودیت شخص<br/><br/>
        /// 
        /// این موجودیت ، رابطه ی "چند به یک" با موجودیتِ
        /// <see cref="T:DataAccess.EntityModule.Class.Entity.PersonEntity" />
        /// دارد . هر رکورد در موجودیت شخص ، میتواند چندین رکورد در موجودیت شماره موبایل داشته باشد .
        /// </summary>
        public virtual PersonEntity PersonEntity { get; set; }


        /// <summary>
        /// کلید خارجی موجودیت شماره موبایل که به کلید اصلی موجودیت شخص اشاره میکند .<br/><br/>
        ///
        /// انتیتی فریم ورک ، اتوماتیک ، فقط کلید خارجی هایی که بصورت صریح در کلاس موجودیت تعریف میشود را هم بصورت index non cluster و هم بصورت not null تنظیم میکند .<br/>
        /// انتیتی فریم ورک ، بخاطر رعایت قوانین نامگذاری برای کلید خارجی ، این پروپرتی را بصورت خودکار ، به عنوان کلید خارجی تعریف میکند .
        /// </summary>
        public int PersonEntityId { get; set; }
    }
}
