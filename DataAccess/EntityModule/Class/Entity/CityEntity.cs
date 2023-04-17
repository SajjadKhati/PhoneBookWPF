using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityModule.Class.Entity
{
    /// <summary>
    /// این موجودیت ، برای ذخیره ی اطلاعات شهرهای مربوط به هر استان کشور در این برنامه هست .<br/><br/>
    /// 
    /// این موجودیت ، رابطه ی "چند به یک" با موجودیت 
    /// <see cref="T:DataAccess.EntityModule.Class.Entity.ProvinceEntity" />
    /// دارد . یک رکورد از موجودیت استان ، میتواند با چند رکورد از این موجودیت (شهر) ، رابطه داشته باشد .<br/>
    /// این جدول ، فقط اگر دیتابیس موجود نبود ، فقط برای یکبار نوشته میشوند .
    /// </summary>
    public class CityEntity
    {
        /// <summary>
        /// شناسه موجودیت شهر
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// نام استان
        /// </summary>
        public string CityName { get; set; }


        /// <summary>
        /// Navigation Property برای موجودیت استان .<br/><br/>
        /// 
        /// این موجودیت ، رابطه ی "چند به یک" با موجودیت 
        /// <see cref="T:DataAccess.EntityModule.Class.Entity.ProvinceEntity" />
        /// دارد . یک رکورد از موجودیت استان ، میتواند با چند رکورد از این موجودیت (شهر) ، رابطه داشته باشد .
        /// </summary>
        public virtual ProvinceEntity ProvinceEntity { get; set; }


        /// <summary>
        /// کلید خارجی موجودیت شهر که به کلید اصلی موجودیت استان اشاره میکند .<br/><br/>
        ///
        /// انتیتی فریم ورک ، اتوماتیک ، فقط کلید خارجی هایی که بصورت صریح در کلاس موجودیت تعریف میشود را هم بصورت index non cluster و هم بصورت not null تنظیم میکند .<br/>
        /// انتیتی فریم ورک ، بخاطر رعایت قوانین نامگذاری برای کلید خارجی ، این پروپرتی را بصورت خودکار ، به عنوان کلید خارجی تعریف میکند .
        /// </summary>
        public int ProvinceEntityId { get; set; }
    }
}
