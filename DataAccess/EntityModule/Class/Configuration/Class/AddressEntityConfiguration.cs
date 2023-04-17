using DataAccess.EntityModule.Class.Configuration.Interface;
using DataAccess.EntityModule.Class.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityModule.Class.Configuration.Class
{
    /// <summary>
    /// پیکربندی موجودیت 
    /// <see cref="T:DataAccess.EntityModule.Class.Entity.AddressEntity" />
    /// در جدول دیتابیس را انجام میدهد .<br/><br/>
    /// 
    /// برای اجرای اصل single responsibility در Solid ، چون وظیفه ی کلاس
    /// <see cref="T:DataAccess.EntityModule.Class.PhoneBookDbContext" />
    /// ، صرفا پیکربندی موجودیت ها نیست ، این وظیفه را به این کلاس محول کردیم (یا همان ، به کلاس هایی که از کلاسِ جنریکِ EntityTypeConfiguration
    /// و اینترفیس
    /// <see cref="T:DataAccess.EntityModule.Class.Configuration.Interface.IEntityConfigurationBase" />
    /// مشتق میشوند) ، محول کردیم .
    /// </summary>
    internal class AddressEntityConfiguration : EntityTypeConfiguration<AddressEntity>, IEntityConfigurationBase
    {
        #region متدهای پیاده سازی شده از Interface


        /// <summary>
        /// تغییر نام جدول برای موجودیت 
        /// <see cref="T:DataAccess.EntityModule.Class.Entity.AddressEntity" /><br/><br/>
        /// 
        /// نام جدول را به "AddressTable" تغییر میدهد .
        /// </summary>
        public void SetTableName()
        {
            this.ToTable("AddressTable");
        }


        /// <summary>
        /// تغییر نوع داده برای صفت ها درون جداول دیتابیس برای موجودیت 
        /// <see cref="T:DataAccess.EntityModule.Class.Entity.AddressEntity" /><br/><br/>
        ///
        /// صفت های Province و City را به نوع "nvarchar(50)" و صفت Address را به نوع "nvarchar(300)" در دیتابیس تغییر میدهد . 
        /// </summary>
        public void SetAttributeDataType()
        {
            this.SetAddressEntityStringDataTypes();
        }


        /// <summary>
        /// الزامی بودن یا همان null نبودن برای صفت را در موجودیتِ
        /// <see cref="T:DataAccess.EntityModule.Class.Entity.AddressEntity" />
        /// ، پیکربندی میکند .<br/><br/>
        ///
        /// صفت های Province و City را به عنوان صفت های غیر قابل nullable ، تنظیم میکند .
        /// </summary>
        public void SetAttributeRequire()
        {
            this.SetAddressEntityStringAttributeRequire();
        }


        #endregion




        //////////////////////////////////////////////////////////////////////////////////////////




        #region متدهای Private


        /// <summary>
        /// تغییر نوع داده های رشته ای در همه ی صفت ها برای موجودیت 
        /// <see cref="T:DataAccess.EntityModule.Class.Entity.MobileNumberEntity" />
        /// </summary>
        private void SetAddressEntityStringDataTypes()
        {
            List<Expression<Func<AddressEntity, string>>> stringExpressions = new List<Expression<Func<AddressEntity, string>>>()
            {
                (AddressEntity addressEntity)=>addressEntity.Province,
                (AddressEntity addressEntity)=>addressEntity.City,
                (AddressEntity addressEntity)=>addressEntity.Address
            };

            for (int iexpressionIndex = 0; iexpressionIndex < stringExpressions.Count; iexpressionIndex++)
            {
                /// دستور زیر ، بررسی میکند اگر مقدار متغییر iexpressionIndex برابر با 2 بود ، مقدار 300 را درون متغییر customStringMaxLength ذخیره میکند وگرنه مقدار 50 را .
                /// در واقع ، برای ستون Address ، حداکثر تعداد کاراکتر را مقدار 150 در نظر میگیرد و برای بقیه ی نوع ستون های رشته ای ، مقدار 50 را در نظر میگیرد .
                int customStringMaxLength = (iexpressionIndex == 2) ? 300 : 50;

                this.SetAttributeStringDataType(stringExpressions[iexpressionIndex], stringMaxLength: customStringMaxLength);
            }
        }


        /// <summary>
        /// تغییر قابلیت null بودن صفت های موجودیتِ 
        /// <see cref="T:DataAccess.EntityModule.Class.Entity.MobileNumberEntity" />
        /// </summary>
        private void SetAddressEntityStringAttributeRequire()
        {
            List<Expression<Func<AddressEntity, string>>> requireStringExpressions =
                new List<Expression<Func<AddressEntity, string>>>()
                {
                    (AddressEntity addressEntity)=>addressEntity.Province,
                    (AddressEntity addressEntity)=>addressEntity.City,
                };

            foreach (Expression<Func<AddressEntity, string>> requireStringExpression in requireStringExpressions)
            {
                this.Property(requireStringExpression).IsRequired();
            }
        }


        #endregion


    }
}
