using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityModule.Class.Entity
{
    /// <summary>
    /// این موجودیت ، برای ذخیره ی اطلاعات استان های کشور در این برنامه هست .<br/><br/>
    /// 
    /// این موجودیت (استان) ، رابطه ی یک به چند با موجودیت 
    /// <see cref="T:DataAccess.EntityModule.Class.Entity.CityEntity" />
    /// دارد . یک رکورد از این موجودیت ، میتواند با چند رکورد از موجودیت شهر در ارتباط باشد .<br/>
    /// این جدول ، فقط اگر دیتابیس موجود نبود ، فقط برای یکبار نوشته میشوند .
    /// </summary>
    public class ProvinceEntity
    {
        /// <summary>
        /// شناسه ی موجودیت استان
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// نام استان
        /// </summary>
        public string ProvinceName { get; set; }


        /// <summary>
        /// Navigation Collection Property برای موجودیت شهر<br/><br/>
        /// 
        /// این موجودیت ، رابطه ی یک به چند با موجودیت شهر دارد .
        /// </summary>
        public virtual ICollection<CityEntity> CityEntities { get; set; }
    }
}
