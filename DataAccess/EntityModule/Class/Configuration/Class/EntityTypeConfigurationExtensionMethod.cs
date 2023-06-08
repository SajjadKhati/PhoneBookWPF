using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.EntityModule.Class.Entity;
using DataAccess.ExceptionModule;

namespace DataAccess.EntityModule.Class.Configuration.Class
{
    /// <summary>
    /// کلاس ExtensionMethod ای هست برای نوعِ جنریکِ کلاسِ EntityTypeConfiguration
    /// که میتواند در کلاس هایی که اینترفیسِ
    /// <see cref="T:DataAccess.EntityModule.Class.Configuration.Interface.IEntityConfigurationAggregate" />
    /// را پیاده سازی میکنند ، مورد استفاده قرار گیرد .
    /// </summary>
    internal static class EntityTypeConfigurationExtensionMethod
    {
        /// <summary>
        /// پیکربندی نوع داده ای رشته ای برای صفت تعیین شده در موجودیت تعیین شده
        /// </summary>
        /// <typeparam name="TEntityType">
        ///  نوع موجودیت تعیین شده
        /// </typeparam>
        /// <param name="entityConfiguration">
        ///  شیِ Extension Method از نوع جنریکِ کلاسِ EntityTypeConfiguration
        /// </param>
        /// <param name="stringExpression">
        ///  عبارت یا اعضا و پروپرتی های تعیین شده به عنوان صفت هایی که به عنوان نوع داده ای رشته ای تعیین میشوند تا نوع داده ای آنها در جدول دیتابیس ، پیکربندی شود .
        /// </param>
        /// <param name="stringType">
        ///  نوع داده ای رشته در زبان sql
        /// </param>
        /// <param name="stringMaxLength">
        ///  حداکثر طول داده ای رشته در sql
        /// </param>
        /// <exception cref="ArgumentNullException"></exception>
        internal static void SetAttributeStringDataType<TEntityType>(this EntityTypeConfiguration<TEntityType> entityConfiguration, 
            Expression<Func<TEntityType, string>> stringExpression, string stringType = "nvarchar", int stringMaxLength = 50)
            where TEntityType : class
        {
            stringExpression = stringExpression ?? throw new ArgumentNullException(nameof(stringExpression),
                ExceptionMessage.argumentNullExceptionMessage);

            entityConfiguration.Property(stringExpression).HasColumnType(stringType).HasMaxLength(stringMaxLength);
        }


        /// <summary>
        /// فهرست یا index ای را برای صفت یا ستون مورد نظر در دیتابیس در نظر میگیرد .
        /// </summary>
        /// <typeparam name="TEntityType">
        ///  نوع موجودیت تعیین شده
        /// </typeparam>
        /// <typeparam name="TPropertyType">
        ///  نوع پروپرتی ای که میخواهید برایش ، index تنظیم کنید .<br/><br/>
        ///
        /// اگر چند پروپرتی یا چند پروپرتی با نوع مختلف بودند ، میتوانید از anonymouse type استفاده کنید و در این صورت در سی شارپ 7.3 ، نباید  مقدارِ
        /// anonymouse type را در متغییری ذخیره کنید و بعد به عنوان ورودی ، برای پارامتر propertyExpression ارسال کنید . بلکه در این نسخه از سی شارپ ، باید مستقیما مقدارِ
        /// anonymouse type را در ورودی پارامترِ propertyExpression ارسال کنید .
        /// برای ذخیره ی anonymouse type درون یک متغییر ، باید از نسخه ی سی شارپ 10 یا بالاتر استفاده کرد .
        /// </typeparam>
        /// <param name="entityConfiguration">
        ///  شیِ Extension Method از نوع جنریکِ کلاسِ EntityTypeConfiguration
        /// </param>
        /// <param name="propertyExpression">
        ///  پروپرتی ای که میخواهید برای صفتِ آن در پایگاه داده ، فهرست یا index ساخته شود .
        /// </param>
        /// <param name="isUnique">
        ///  مشخص میکند که آیا ، مقدار هر رکورد از پروپرتی یا ستون (هایی) که به عنوان index تعیین میشود ، منحصر به فرد هست یا نه .
        /// </param>
        /// <param name="isClustered">
        ///  تعیین میکند که آیا نوع index مان ، یک فهرست خوشه ای هست یا نه .
        /// </param>
        /// <exception cref="ArgumentNullException"></exception>
        internal static void SetAttributeIndex<TEntityType, TPropertyType>(this EntityTypeConfiguration<TEntityType> entityConfiguration,
            Expression<Func<TEntityType, TPropertyType>> propertyExpression, bool isUnique = false, bool isClustered = false)
            where TEntityType : class
        {
            propertyExpression = propertyExpression ?? throw new ArgumentNullException(nameof(propertyExpression),
                ExceptionMessage.argumentNullExceptionMessage);

            entityConfiguration.HasIndex(propertyExpression).IsUnique(isUnique).IsClustered(isClustered);
        }


        /// <summary>
        /// فهرست یا index ای را برای صفت یا ستون مورد نظر در دیتابیس با نام خاصی در نظر میگیرد .
        /// </summary>
        /// <typeparam name="TEntityType">
        ///  نوع موجودیت تعیین شده
        /// </typeparam>
        /// <typeparam name="TPropertyType">
        ///  نوع پروپرتی ای که میخواهید برایش ، index تنظیم کنید .<br/><br/>
        ///
        /// اگر چند پروپرتی یا چند پروپرتی با نوع مختلف بودند ، میتوانید از anonymouse type استفاده کنید و در این صورت در سی شارپ 7.3 ، نباید  مقدارِ
        /// anonymouse type را در متغییری ذخیره کنید و بعد به عنوان ورودی ، برای پارامتر propertyExpression ارسال کنید . بلکه در این نسخه از سی شارپ ، باید مستقیما مقدارِ
        /// anonymouse type را در ورودی پارامترِ propertyExpression ارسال کنید .
        /// برای ذخیره ی anonymouse type درون یک متغییر ، باید از نسخه ی سی شارپ 10 یا بالاتر استفاده کرد .
        /// </typeparam>
        /// <param name="entityConfiguration">
        ///  شیِ Extension Method از نوع جنریکِ کلاسِ EntityTypeConfiguration
        /// </param>
        /// <param name="propertyExpression">
        ///  پروپرتی ای که میخواهید برای صفتِ آن در پایگاه داده ، فهرست یا index ساخته شود .
        /// </param>
        /// <param name="indexName">
        ///  نام index . <br/>
        /// نام ، نباید null ارسال شود .
        /// </param>
        /// <param name="isUnique">
        ///  مشخص میکند که آیا ، مقدار هر رکورد از پروپرتی یا ستون (هایی) که به عنوان index تعیین میشود ، منحصر به فرد هست یا نه .
        /// </param>
        /// <param name="isClustered">
        ///  تعیین میکند که آیا نوع index مان ، یک فهرست خوشه ای هست یا نه .
        /// </param>
        /// <exception cref="ArgumentNullException"></exception>
        internal static void SetAttributeIndex<TEntityType, TPropertyType>(this EntityTypeConfiguration<TEntityType> entityConfiguration,
            Expression<Func<TEntityType, TPropertyType>> propertyExpression, string indexName, bool isUnique = false, bool isClustered = false)
            where TEntityType : class
        {
            propertyExpression = propertyExpression ?? throw new ArgumentNullException(nameof(propertyExpression),
                ExceptionMessage.argumentNullExceptionMessage);
            indexName = indexName ?? throw new ArgumentNullException(nameof(indexName),
                ExceptionMessage.argumentNullExceptionMessage);

            entityConfiguration.HasIndex(propertyExpression).HasName(indexName).IsUnique(isUnique).IsClustered(isClustered);
        }


    }
}
