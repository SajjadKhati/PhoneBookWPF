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
    /// <see cref="T:DataAccess.EntityModule.Class.Entity.CityEntity" />
    /// در جدول دیتابیس را انجام میدهد .<br/><br/>
    /// 
    /// برای اجرای اصل single responsibility در Solid ، چون وظیفه ی کلاس
    /// <see cref="T:DataAccess.EntityModule.Class.PhoneBookDbContext" />
    /// ، صرفا پیکربندی موجودیت ها نیست ، این وظیفه را به این کلاس محول کردیم (یا همان ، به کلاس هایی که از کلاسِ جنریکِ EntityTypeConfiguration
    /// و اینترفیس
    /// <see cref="T:DataAccess.EntityModule.Class.Configuration.Interface.IEntityConfigurationAggregate" />
    /// مشتق میشوند) ، محول کردیم .
    /// </summary>
    internal class CityEntityConfiguration : EntityTypeConfiguration<CityEntity>, IEntityConfigurationAggregate
    {
        #region متدهای پیاده سازی شده از Interface


        /// <summary>
        /// تغییر نام جدول برای موجودیت 
        /// <see cref="T:DataAccess.EntityModule.Class.Entity.CityEntity" /><br/><br/>
        /// 
        /// نام جدول را به "CityTable" و شِمای آنرا به "ProvinceList" تغییر میدهد .
        public void SetTableName()
        {
            this.ToTable("CityTable", "ProvinceList");
        }


        /// <summary>
        /// تغییر نوع داده برای صفت ها درون جداول دیتابیس برای موجودیت 
        /// <see cref="T:DataAccess.EntityModule.Class.Entity.CityEntity" /><br/><br/>
        /// 
        /// صفت و پروپرتی CityName از نوع string را به نوع "nvarchar(50)" تغییر میدهد . 
        public void SetAttributeDataType()
        {
            this.SetCityEntityStringDataTypes();
        }


        /// <summary>
        /// الزامی بودن یا همان null نبودن برای صفت را در موجودیتِ
        /// <see cref="T:DataAccess.EntityModule.Class.Entity.CityEntity" />
        /// ، پیکربندی میکند .<br/><br/>
        /// 
        /// صفت و پروپرتیِ CityName را از حالت nullable خارج میکند و نیازش را ضروری میسازد چون نباید خالی باشد
        /// </summary>
        public void SetAttributeRequire()
        {
            this.SetCityEntityStringAttributeRequire();
        }


        #endregion




        //////////////////////////////////////////////////////////////////////////////////////////




        #region متدهای Private


        /// <summary>
        /// تغییر نوع داده های رشته ای در همه ی صفت ها برای موجودیت 
        /// <see cref="T:DataAccess.EntityModule.Class.Entity.CityEntity" />
        /// </summary>
        private void SetCityEntityStringDataTypes()
        {
            this.SetAttributeStringDataType((CityEntity cityEntity) => cityEntity.CityName);
        }


        /// <summary>
        /// تغییر قابلیت null بودن صفت های موجودیتِ 
        /// <see cref="T:DataAccess.EntityModule.Class.Entity.CityEntity" />
        /// </summary>
        private void SetCityEntityStringAttributeRequire()
        {
            this.Property((CityEntity cityEntity) => cityEntity.CityName).IsRequired();
        }


        #endregion


    }
}
