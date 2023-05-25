using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityModule.Class.Entity
{
    /// <summary>
    /// موجودیت شخص<br/><br/>
    /// 
    /// این موجودیت ، با موجودیتِ
    /// <see cref="T:DataAccess.EntityModule.Class.Entity.MobileNumberEntity" />
    /// رابطه یک یک به چند دارد .<br/>
    /// همچنین این موجودیت ، با موجودیت 
    /// <see cref="T:DataAccess.EntityModule.Class.Entity.AddressEntity" />
    /// رابطه ی چند به چند دارد و هر رکورد از موودیت شخص ، میتواند چندین رکورد از موجودیتِ آدرس داشته باشد و هر آدرس (خانه) هم متعلق به چندین رکورد از موجودیتِ شخص باشد .
    /// </summary>
    public class PersonEntity
    {
        /// <summary>
        /// شناسه ی موجودیت شخص
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// نام شخص
        /// </summary>
        public string FirstName { get; set; }


        /// <summary>
        /// نام خانوادگی شخص
        /// </summary>
        public string LastName { get; set; }


        /// <summary>
        /// Navigation Collection Property برای شماره موبایل شخص<br/><br/>
        /// این موجودیت (شخص) ، رابطه ی یک به چند با موجودیت 
        /// <see cref="T:DataAccess.EntityModule.Class.Entity.MobileNumberEntity" />
        /// دارد . هر رکورد در موجودیت شخص ، میتواند چندین رکورد در موجودیت شماره موبایل داشته باشد<br/>
        /// بخاطر lazy loading ، بصورت virtual تعریف شده .
        /// </summary>
        public virtual ICollection<MobileNumberEntity> MobileNumbers { get; set; }


        /// <summary>
        /// Navigation Collection Property برای آدرس شخص<br/><br/>
        /// 
        /// این موجودیت ، رابطه ی "چند به چند" با موجودیتِ 
        /// <see cref="T:DataAccess.EntityModule.Class.Entity.AddressEntity" />
        /// دارد . هم ، هر رکورد در موجودیت شخص میتواند چندین رکورد در موجودیت آدرس داشته باشد 
        /// و هم اینکه هر رکورد در موجودیت آدرس هم متعلق به چندین رکورد از موجودیت شخص میتواند باشد .<br/><br/>
        /// 
        /// اما از آنجا که صفت یا ستون یا پروپرتی ای در هر کدام از این موجودیت ها وجود ندارد که مربوط به یک رکورد خاص از موجودیتِ طرف دیگر باشد ،
        /// یعنی ، حتی یک صفت در این موجودیت پیدا نمیشود که با حداکثر ، یک رکورد از یک صفت در موجودیت 
        /// <see cref="T:DataAccess.EntityModule.Class.Entity.AddressEntity" />
        /// مربوط باشد ، بلکه ممکن است که با بیشتر از یک رکورد آنها در ارتباط باشد ،
        /// بنابراین نیازی به ایجاد جدول مشترکی که نیاز به تعریف آن صفت در آن جدول مشترک باشد ، نیست .<br/><br/>
        /// 
        /// پس نیازی نداریم که بصورت صریح ، آن جدول مشترک را تعریف کنیم چون آن جدول مشترک ، فقط شامل کلیدها هستند و شامل اطلاعات داده ای نیستند .
        /// بنابراین همین قدر کافی هست که خود EF ، برای مان بصورت ضمنی آن جدول مشترک را ایجاد کند .<br/><br/>
        /// 
        /// برای استفاده از lazy loading ، این عضو از Navigation Collection Property را بصورت virtual تعریف کردیم .
        /// </summary>
        public virtual ICollection<AddressEntity> Addresses { get; set; }
    }

}
