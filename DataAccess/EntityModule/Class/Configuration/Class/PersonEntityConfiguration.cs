using DataAccess.EntityModule.Class.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.EntityModule.Class.Configuration.Interface;


namespace DataAccess.EntityModule.Class.Configuration.Class
{
    /// <summary>
    /// پیکربندی موجودیت 
    /// <see cref="T:DataAccess.EntityModule.Class.Entity.PersonEntity" />
    /// در جدول دیتابیس را انجام میدهد .<br/><br/>
    /// 
    /// برای اجرای اصل single responsibility در Solid ، چون وظیفه ی کلاس
    /// <see cref="T:DataAccess.EntityModule.Class.PhoneBookDbContext" />
    /// ، صرفا پیکربندی موجودیت ها نیست ، این وظیفه را به این کلاس محول کردیم (یا همان ، به کلاس هایی که از کلاسِ جنریکِ EntityTypeConfiguration
    /// و اینترفیس
    /// <see cref="T:DataAccess.EntityModule.Class.Configuration.Interface.IEntityConfigurationBase" />
    /// مشتق میشوند) ، محول کردیم .
    /// </summary>
    internal class PersonEntityConfiguration : EntityTypeConfiguration<PersonEntity>, IEntityConfigurationBase, IAttributeIndexConfiguration
    {
        #region متدهای پیاده سازی شده از Interface


        /// <summary>
        /// تغییر نام جدول برای موجودیت 
        /// <see cref="T:DataAccess.EntityModule.Class.Entity.PersonEntity" /><br/><br/>
        /// 
        /// نام جدول را به "PersonTable" تغییر میدهد .
        /// </summary>
        public void SetTableName()
        {
            this.ToTable("PersonTable");
        }


        /// <summary>
        /// تغییر نوع داده برای صفت ها درون جداول دیتابیس برای موجودیت 
        /// <see cref="T:DataAccess.EntityModule.Class.Entity.PersonEntity" /><br/><br/>
        /// 
        /// همه ی صفت و پروپرتی های از نوع string را به نوع "nvarchar(50)" تغییر میدهد . هم پروپرتی FirstName و هم LastName را .
        /// </summary>
        public void SetAttributeDataType()
        {
            this.SetPersonEntityStringDataTypes();
        }


        /// <summary>
        /// الزامی بودن یا همان null نبودن برای صفت را در موجودیتِ
        /// <see cref="T:DataAccess.EntityModule.Class.Entity.PersonEntity" />
        /// ، پیکربندی میکند .<br/><br/>
        /// 
        /// هر 2 صفت و پروپرتیِ FirstName و  LastName را از حالت nullable خارج میکند و نیازش را ضروری میسازد چون نباید هیچ کدام شان خالی باشند
        /// </summary>
        public void SetAttributeRequire()
        {
            this.SetPersonEntityStringAttributeRequire();
        }


        /// <summary>
        /// ایجاد فهرست یا index برای صفت های موجودیت
        /// <see cref="T:DataAccess.EntityModule.Class.Entity.PersonEntity" /><br/><br/>
        /// 
        /// برای دو صفت و پروپرتی های FirstName و LastName فهرست ترکیبی یا composite index ایجاد مینماید .<br/>
        /// یعنی یک index درست میکند که ترکیبی برای هر دو صفت هست .<br/><br/>
        /// 
        /// بخاطر این صفت ترکیبی درست کردیم چون در دفترچه تلفن ، جستجوها بر اساس هم نام و هم نام خانوداگی ، بسیار زیاد اتفاق میافتد .
        /// </summary>
        public void SetAttributeIndex()
        {
            string indexName = "FirstName_LastName_Index";
            this.SetAttributeIndex((PersonEntity personEntity) =>  new { personEntity.FirstName, personEntity.LastName }, indexName);
        }


        #endregion




        //////////////////////////////////////////////////////////////////////////////////////////




        #region متدهای private


        /// <summary>
        /// تغییر نوع داده های رشته ای در همه ی صفت ها برای موجودیت 
        /// <see cref="T:DataAccess.EntityModule.Class.Entity.PersonEntity" />
        /// </summary>
        private void SetPersonEntityStringDataTypes()
        {
            List<Expression<Func<PersonEntity, string>>> stringExpressions = this.GetPersonEntityStringDataTypesExpressions();
            
            foreach (Expression<Func<PersonEntity, string>> stringExpression in stringExpressions)
            {
                this.SetAttributeStringDataType(stringExpression);
            }
        }


        /// <summary>
        /// تغییر قابلیت null بودن صفت های موجودیتِ 
        /// <see cref="T:DataAccess.EntityModule.Class.Entity.PersonEntity" />
        /// </summary>
        private void SetPersonEntityStringAttributeRequire()
        {
            List<Expression<Func<PersonEntity, string>>> stringExpressions = this.GetPersonEntityStringDataTypesExpressions();

            foreach (Expression<Func<PersonEntity, string>> stringExpression in stringExpressions)
            {
                /// متد IsRequired ای که توسط فراخوانی متد Property در دسترس قرار میگیرد ، پیکربندی non-nullable را برای صفت مورد نظر در جدول دیتابیس انجام میدهد .
                /// اما متد HasRequire که EntityTypeConfiguration هست ، ربطی به این موضوع ندارد و برای پیکربندی رابطه ی بین جدول ها استفاده میشود .
                this.Property(stringExpression).IsRequired();
            }
        }


        /// <summary>
        /// شیِ Expression یا عبارت و اطلاعات پروپرتی های از نوع string را در موجودیت 
        /// <see cref="T:DataAccess.EntityModule.Class.Entity.PersonEntity" />
        /// را میگیرد .
        /// </summary>
        /// <returns></returns>
        private List<Expression<Func<PersonEntity, string>>> GetPersonEntityStringDataTypesExpressions()
        {
            List<Expression<Func<PersonEntity, string>>> stringExpressions = new List<Expression<Func<PersonEntity, string>>>()
            {
                (PersonEntity personEntity) => personEntity.FirstName,
                (PersonEntity personEntity) => personEntity.LastName
            };
            return stringExpressions;
        }


        #endregion



    }
}
