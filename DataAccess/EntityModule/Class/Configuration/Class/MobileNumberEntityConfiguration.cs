using DataAccess.EntityModule.Class.Configuration.Interface;
using DataAccess.EntityModule.Class.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq.Expressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityModule.Class.Configuration.Class
{
    /// <summary>
    /// پیکربندی موجودیت 
    /// <see cref="T:DataAccess.EntityModule.Class.Entity.MobileNumberEntity" />
    /// در جدول دیتابیس را انجام میدهد .<br/><br/>
    /// 
    /// برای اجرای اصل single responsibility در Solid ، چون وظیفه ی کلاس
    /// <see cref="T:DataAccess.EntityModule.Class.PhoneBookDbContext" />
    /// ، صرفا پیکربندی موجودیت ها نیست ، این وظیفه را به این کلاس محول کردیم (یا همان ، به کلاس هایی که از کلاسِ جنریکِ EntityTypeConfiguration
    /// و اینترفیس
    /// <see cref="T:DataAccess.EntityModule.Class.Configuration.Interface.IEntityConfigurationAggregate" />
    /// مشتق میشوند) ، محول کردیم .
    /// </summary>
    internal class MobileNumberEntityConfiguration : EntityTypeConfiguration<MobileNumberEntity>,
        IEntityConfigurationAggregate, IAttributeIndexConfiguration
    {

        #region متدهای پیاده سازی شده از Interface


        /// <summary>
        /// تغییر نام جدول برای موجودیت 
        /// <see cref="T:DataAccess.EntityModule.Class.Entity.MobileNumberEntity" /><br/><br/>
        /// 
        /// نام جدول را به "MobileNumberTable" تغییر میدهد .
        /// </summary>
        public void SetTableName()
        {
            this.ToTable("MobileNumberTable");
        }


        /// <summary>
        /// تغییر نوع داده برای صفت ها درون جداول دیتابیس برای موجودیتِ
        /// <see cref="T:DataAccess.EntityModule.Class.Entity.MobileNumberEntity" /><br/><br/>
        ///
        /// صفت و پروپرتیِ MobileNumber را به نوع "varchar(11)" تغییر میدهد . چون شماره موبایل ، 11 رقم است .<br/>
        /// از این بابت "var" را در برایش انتخاب کردیم چون هر چند در این نسخه ، مقدار پیش فرض ای برای این ستون و همچنین nullable بودن ، برایش انتخاب نشد ،
        /// اما اگر در آینده ، نوع پیش فرضی برایش در نظر گرفته شود و همچنین nullable شود ، مشکل (ایجاد فضای خالی ، برای آن رکوردهایی که null هستند) ، بوجود نیاید .
        /// </summary>
        public void SetAttributeDataType()
        {
            this.SetMobileNumberEntityStringDataTypes();
        }


        /// <summary>
        /// الزامی بودن یا همان null نبودن برای صفت را در موجودیتِ
        /// <see cref="T:DataAccess.EntityModule.Class.Entity.MobileNumberEntity" />
        /// ، پیکربندی میکند .<br/><br/>
        /// 
        /// صفت و پروپرتیِ MobileNumber را از حالت nullable خارج میکند و نیازش را ضروری میسازد چون نباید خالی باشد
        /// </summary>
        public void SetAttributeRequire()
        {
            this.SetMobileNumberEntityAttributeRequire();
        }


        /// <summary>
        /// ایجاد فهرست منحصر به فرد یا index Unique برای صفتی در موجودیت
        /// <see cref="T:DataAccess.EntityModule.Class.Entity.MobileNumberEntity" /><br/><br/>
        /// 
        /// برای صفت و پروپرتی MobileNumber فهرست منحصر به فرد ایجاد مینماید چون مقدار هیچ یک از رکوردهای شماره موبایل ، با رکورد دیگری ، یکسان نیست .<br/>
        /// </summary>
        public void SetAttributeIndex()
        {
            this.SetMobileNumberEntityAttributeIndex();
        }


        #endregion




        //////////////////////////////////////////////////////////////////////////////////////////
        
        


        #region  متدهای private


        /// <summary>
        /// تغییر نوع داده های رشته ای در صفتی برای موجودیت 
        /// <see cref="T:DataAccess.EntityModule.Class.Entity.MobileNumberEntity" />
        /// </summary>
        private void SetMobileNumberEntityStringDataTypes()
        {
            string databaseStringType = "varchar";
            int databaseStringMaxLength = 11;

            this.SetAttributeStringDataType((MobileNumberEntity mobileNumberEntity) => mobileNumberEntity.MobileNumber,
                databaseStringType, databaseStringMaxLength);
        }


        /// <summary>
        /// تغییر قابلیت null بودن صفت موجودیتِ 
        /// <see cref="T:DataAccess.EntityModule.Class.Entity.MobileNumberEntity" />
        /// </summary>
        private void SetMobileNumberEntityAttributeRequire()
        {
            this.Property((MobileNumberEntity mobileNumberEntity) => mobileNumberEntity.MobileNumber).IsRequired();
        }


        /// <summary>
        /// برای ستون MobileNumber ، یک index Unique برای موجودیتِ
        /// <see cref="T:DataAccess.EntityModule.Class.Entity.MobileNumberEntity" />
        /// میسازد .<br/><br/>
        /// 
        /// چون شماره موبایل هیچ رکوردی شبیه به هم نیست ، پس ، این ستون را به عنوان index Unique در نظر میگیریم .<br/>
        /// چون برای ایجاد یک Unique ، باید یک Index بسازیم ، بنابراین ، از همان اینترفیسِ
        /// <see cref="T:DataAccess.EntityModule.Class.Configuration.Interface.IAttributeIndexConfiguration" />
        /// استفاده کردیم .
        /// </summary>
        private void SetMobileNumberEntityAttributeIndex()
        {
            string indexUniqueName = "MobileNumber_UniqueIndex";
            bool isUnique = true;

            this.SetAttributeIndex((MobileNumberEntity mobileNumberEntity) => mobileNumberEntity.MobileNumber,
                indexUniqueName, isUnique);
        }


        #endregion


    }
}
