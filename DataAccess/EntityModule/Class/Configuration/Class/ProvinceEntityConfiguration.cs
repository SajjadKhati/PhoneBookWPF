using DataAccess.EntityModule.Class.Configuration.Interface;
using DataAccess.EntityModule.Class.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityModule.Class.Configuration.Class
{
    /// <summary>
    /// پیکربندی موجودیت 
    /// <see cref="T:DataAccess.EntityModule.Class.Entity.ProvinceEntity" />
    /// در جدول دیتابیس را انجام میدهد .<br/><br/>
    /// 
    /// برای اجرای اصل single responsibility در Solid ، چون وظیفه ی کلاس
    /// <see cref="T:DataAccess.EntityModule.Class.PhoneBookDbContext" />
    /// ، صرفا پیکربندی موجودیت ها نیست ، این وظیفه را به این کلاس محول کردیم (یا همان ، به کلاس هایی که از کلاسِ جنریکِ EntityTypeConfiguration
    /// و اینترفیس
    /// <see cref="T:DataAccess.EntityModule.Class.Configuration.Interface.IEntityConfigurationBase" />
    /// مشتق میشوند) ، محول کردیم .
    /// </summary>
    internal class ProvinceEntityConfiguration : EntityTypeConfiguration<ProvinceEntity>, IEntityConfigurationBase
    {

        #region متدهای پیاده سازی شده از Interface


        /// <summary>
        /// تغییر نام جدول برای موجودیت 
        /// <see cref="T:DataAccess.EntityModule.Class.Entity.ProvinceEntity" /><br/><br/>
        /// 
        /// نام جدول را به "ProvinceTable" و شِمای آنرا به "ProvinceList" تغییر میدهد .
        /// </summary>
        public void SetTableName()
        {
            this.ToTable("ProvinceTable", "ProvinceList");
        }


        /// <summary>
        /// تغییر نوع داده برای صفت ها درون جداول دیتابیس برای موجودیت 
        /// <see cref="T:DataAccess.EntityModule.Class.Entity.ProvinceEntity" /><br/><br/>
        /// 
        /// صفت و پروپرتی ProvinceName از نوع string را به نوع "nvarchar(50)" تغییر میدهد . 
        /// </summary>
        public void SetAttributeDataType()
        {
            this.SetProvinceEntityStringDataTypes();
        }


        /// <summary>
        /// الزامی بودن یا همان null نبودن برای صفت را در موجودیتِ
        /// <see cref="T:DataAccess.EntityModule.Class.Entity.ProvinceEntity" />
        /// ، پیکربندی میکند .<br/><br/>
        /// 
        /// صفت و پروپرتیِ ProvinceName را از حالت nullable خارج میکند و نیازش را ضروری میسازد چون نباید خالی باشد
        /// </summary>
        public void SetAttributeRequire()
        {
            this.SetProvinceEntityStringAttributeRequire();
        }


        #endregion




        //////////////////////////////////////////////////////////////////////////////////////////




        #region متدهای Private


        /// <summary>
        /// تغییر نوع داده های رشته ای در همه ی صفت ها برای موجودیت 
        /// <see cref="T:DataAccess.EntityModule.Class.Entity.ProvinceEntity" />
        /// </summary>
        private void SetProvinceEntityStringDataTypes()
        {
            this.SetAttributeStringDataType((ProvinceEntity provinceEntity) => provinceEntity.ProvinceName);
        }


        /// <summary>
        /// تغییر قابلیت null بودن صفت های موجودیتِ 
        /// <see cref="T:DataAccess.EntityModule.Class.Entity.ProvinceEntity" />
        /// </summary>
        private void SetProvinceEntityStringAttributeRequire()
        {
            this.Property((ProvinceEntity provinceEntity) => provinceEntity.ProvinceName).IsRequired();
        }


        #endregion


    }
}
