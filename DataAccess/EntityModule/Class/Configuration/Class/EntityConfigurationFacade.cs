using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.EntityModule.Class.Configuration.Interface;

namespace DataAccess.EntityModule.Class.Configuration.Class
{
    /// <summary>
    /// کلاسی که با استفاده از الگوی طراحی Facade ، برای کم کردن پیچیدگی های فراخوانی و استفاده از کلاس هایی که اینترفیس
    /// <see cref="T:DataAccess.EntityModule.Class.Configuration.Interface.IEntityConfigurationBase" />
    /// را پیاده سازی میکنند ، 
    /// درون کلاس
    ///<see cref="T:DataAccess.EntityModule.Class.PhoneBookDbContext" />
    /// طراحی شد .<br/><br/>
    /// 
    /// در واقع این کلاس ، رابطی میان کلاسِ
    /// <see cref="T:DataAccess.EntityModule.Class.PhoneBookDbContext" />
    /// با کلاس هایی که اینترفیس
    /// <see cref="T:DataAccess.EntityModule.Class.Configuration.Interface.IEntityConfigurationBase" />
    /// را پیاده سازی میکنند هست تا پیچیدگی استفاده از این کلاس ها را برای کلاس
    /// <see cref="T:DataAccess.EntityModule.Class.PhoneBookDbContext" />
    /// کمتر کند .
    /// </summary>
    internal class EntityConfigurationFacade
    {
        #region فیلد


        /// <summary>
        /// متغییر سراسری ای که لیستی از IEntityConfigurationBase را برای فراخوانی در بعضی متدها ذخیره میکند .
        /// </summary>
        private List<IEntityConfigurationBase> entityConfigurations;


        #endregion




        //////////////////////////////////////////////////////////////////////////////////////////




        #region متدها


        #region متد سازنده


        /// <summary>
        /// متد سازنده ی این کلاس که شی ای از لیست
        /// <see cref="T:DataAccess.EntityModule.Class.Configuration.Interface.IEntityConfigurationBase" />
        /// را برای فراخوانی متدهایش استفاده میکند
        /// </summary>
        /// <param name="entityConfigurations">
        ///  لیست
        /// <see cref="T:DataAccess.EntityModule.Class.Configuration.Interface.IEntityConfigurationBase" />
        /// </param>
        internal EntityConfigurationFacade(List<IEntityConfigurationBase> entityConfigurations)
        {
            this.entityConfigurations = entityConfigurations;
        }


        #endregion


        //////////////////////////////////////////////////////////////////////////////////////////


        #region متدهای غیر شخصی


        /// <summary>
        /// همه ی متدهای آیتم های لیست اشیاء
        /// <see cref="T:DataAccess.EntityModule.Class.Configuration.Interface.IEntityConfigurationBase" />
        /// را که در متد سازنده گرفته شده بود را برای پیکربندی دیتابیس ، فراخوانی میکند .
        /// </summary>
        internal void SetEntityConfigurations()
        {
            foreach (IEntityConfigurationBase entityConfiguration in this.entityConfigurations)
            {
                this.CallEntityConfiguration(entityConfiguration);
            }
        }


        #endregion


        //////////////////////////////////////////////////////////////////////////////////////////


        #region متدهای Private


        /// <summary>
        /// متدهای کلاسی که اینترفیس
        /// <see cref="T:DataAccess.EntityModule.Class.Configuration.Interface.IEntityConfigurationBase" />
        /// را فراخوانی کرده بود را برای پیکربندی فراخوانی میکند .
        /// </summary>
        /// <param name="entityConfiguration"></param>
        private void CallEntityConfiguration(IEntityConfigurationBase entityConfiguration)
        {
            entityConfiguration.SetTableName();
            entityConfiguration.SetAttributeDataType();
            entityConfiguration.SetAttributeRequire();

            /// بررسی میکند که شیِ entityConfiguration ، از نوع اینترفیسِ IAttributeIndexConfiguration هست یا نه .
            /// اگر درست بود ، به نوعِ این اینترفیس تبدیل میکند و در متغییری که بعد از اینترفیس آمد ، یعنی در متغییر indexConfiguration ذخیره میکند .
            if (entityConfiguration is IAttributeIndexConfiguration indexConfiguration)
                indexConfiguration.SetAttributeIndex();
        }


        #endregion

        #endregion


    }
}
