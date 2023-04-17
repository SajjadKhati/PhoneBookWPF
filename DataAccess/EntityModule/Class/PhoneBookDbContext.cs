using System;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.ModelConfiguration;
using DataAccess.EntityModule.Class.Configuration;
using DataAccess.EntityModule.Class.Entity;
using DataAccess.EntityModule.Class.Configuration.Class;
using DataAccess.EntityModule.Class.Configuration.Interface;
using System.Data.Entity.Core.Metadata.Edm;
using DataAccess.ReflectionModule;

namespace DataAccess.EntityModule.Class
{
    /// <summary>
    /// اين کلاس براي ارتباط و مدل سازي ديتابيس در اِنتیتی فریم ورک است .<br/><br/>
    /// 
    /// هر چند اگر این کلاس را بصورت جنریک تعریف میکردیم تا موجودیت ها را بجای اینکه در این لایه تعریف کنیم ، در لایه ی Model تعریف میکردیم ، 
    /// که در این صورت باعث میشد که کلاس ها (چه کلاس های مربوط به موجودیت ها و یا کلاس هایی که در لایه ی Model تعریف میشوند) ، 
    /// فقط یکبار و آن هم فقط در لایه ی Model تعریف شوند ، و به این ترتیب ، حجم کدنویسی برای ایجاد کلاس ها ،  کمتر میشد ، <br/><br/>
    /// 
    /// اما مشکل مهم در این روند ،
    /// این است که رابطه ی موجودیت ها در موجودیت های انتیتی فریم ورک ، مثل روابط در جداول و موجودیت های پایگاه داده ،بر اساس "چندی" شکل میگیرد 
    /// اما روابط در کلاس های لایه ی Model ، بر اساس سه نوع روابط composition و aggregation و association شکل میگیرد که 
    /// مخصوصا اگر طراحی کلاس های موجودیت های انتیتی فریم ورک و همچنین طراحی کلاس ها در لایه ی Model ، هر چه پیچیده تر شود ،
    /// امکان اینکه نوع روابط در کلاس های لایه ی Model با موجودیت های کلاس های انتیتی فریم ورک متفاوت باشد ، بیشتر میشود
    /// که در این صورت ، حداقل ، باعث نقص اصل single responsibility در Solid میشود .
    /// </summary>
    public class PhoneBookDbContext : DbContext
    {
        /// <summary>
        /// اين متد سازنده ، که يکي از اورلودهاي متد سازنده ي پدرش که پارامتري بنام nameOrConnectionString که از نوع رشته هست را فراخواني ميکند ،
        /// اگر در ابتداي مقداري که به عنوان رشته اش ميدهيم ، عبارت "name=" را بنويسيم ، ميرود در فايل App.Config _در پروژه هاي وب ، نام متفاوت دارد_ اي که
        /// که درون پروژه ي اي که بصورت پيش فرض اجرا ميشود ، دنبال تگِ connectionStrings اي که مقدار name اش ، برابر با مقداري که در اين متد سازنده داريم ، ميگردد .<br/><br/>
        /// 
        /// در اينجا ، دنبال تگِ connectionStrings اي که مقدار PhoneBookDbContext آن هم درون فايل App.Config اي که در پروژه اي که بصورت پيش فرض که در اينجا
        /// همان لايه و پروژه ي PhoonBook هست ، ميگردد .
        ///
        /// اگر عبارت "name=" را در ابتداي رشته اش نگذاريم ، مستقيما آن را به عنوان مقدار Connection String در نظر گرفته و به ديتابيس اي که آن Connection String را دارد ، -
        /// متصل ميشود .
        /// </summary>
        public PhoneBookDbContext() : base("name=PhoneBookDbContext")
        {

        }


        /// <summary>
        /// هر وقت model مان در حال مقداردهي اوليه و ساخته شدن باشد ، اين متد اجرا ميشود .
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            /// پيکربندي هايي که توسط fluent api ها ، در کلاس هايي که اينترفيس IEntityConfigurationBase را پياده سازي ميکنند ، انجام داديم را روي ديتابيس مان اِعمال ميکنند .
            this.AddEntityConfiguration(modelBuilder.Configurations);

            base.OnModelCreating(modelBuilder);
        }


        /// <summary>
        /// تنظيماتي که در کلاس هايي که اينترفيس
        /// <see cref="T:DataAccess.EntityModule.Class.Configuration.Interface.IEntityConfigurationBase" />
        /// را براي پيکربندي ديتابيس ، پياده سازي کردند را به شيِ ConfigurationRegistrar اضافه ميکند .
        /// </summary>
        /// <param name="configurationRegister">
        ///  شيِ
        /// <see cref="T:System.Data.Entity.ModelConfiguration.Configuration.ConfigurationRegistrar" />
        /// اي که بايد تنظيماتي که در کلاس هايي که اينترفيسِ
        /// <see cref="T:DataAccess.EntityModule.Class.Configuration.Interface.IEntityConfigurationBase" />
        /// پياده سازي شدند را به آن اضافه کرد .
        /// </param>
        private void AddEntityConfiguration(ConfigurationRegistrar configurationRegister)
        {
            /// ليستي از شي کلاس هايي که اينترفيس IEntityConfigurationBase را پياده سازي کردند را در زمان اجراي برنامه و توسط Reflection برميگرداند .
            List<IEntityConfigurationBase> entityConfigurations = this.CreateEntityConfigurations();
            if (entityConfigurations == null)
                return;

            /// توسط کلاس Facade اي که براي ارتباط با کلاس هايي هست که اينترفيس IEntityConfigurationBase را پياده سازي کردند ، -
            /// از اين کلاس ها براي پيکربندي ديتابيس استفاده ميکنيم . اما در ادامه بايد اين پيکربندي را براي اِعمال در ديتابيس ، به شيِ configurationRegister اضافه کنيم .
            EntityConfigurationFacade entityConfigurationFacade = new EntityConfigurationFacade(entityConfigurations);
            entityConfigurationFacade.SetEntityConfigurations();

            /// پيکربندي انجام شده را براي اِعمال در ديتابيس مان ، به شيِ configurationRegister اضافه ميکنيم تا EF ، اين تنظيمات را روي ديتابيس مان اِعمال کند .
            foreach (IEntityConfigurationBase currentEntityConfiguration in entityConfigurations)
            {
                ///  دقت شود که موقع کمپايل ، مثلا در آيتم و يا ايندکس صفر ام از ليست entityConfigurations که شيِ PersonEntityConfiguration ولي -
                /// ولي نوع داده اي اش از نوع اينترفيس IEntityConfigurationBase هست ؛ اين نوع اينترفيس را بصورت ضمني ، به نوع PersonEntityConfiguration  -
                /// نميتواند تبديل کند . و بخاطر همين ، خطاي زمان کمپايل ميگيرد .
                /// 
                /// از طرفي خودمان هم نميتوانيم صريحا به نوع PersonEntityConfiguration تبديل کنيم .
                /// چون در هر بار از ليستِ entityConfigurations ، شيِ متفاوتي خواهيم داشت . پس ، تنها يک راهکار هست که نوع و مقداردهي آن ، زمان اجراي برنامه مشخص شود .
                /// راحت ترين روش براي اين کار ، با استفاده از کلمه کليدي dynamic وگرنه روش هاي ديگري مثل Reflection هست که نسبت به اين ، کار را سخت تر ميکند .
                configurationRegister.Add((dynamic)currentEntityConfiguration);
            }
        }


        /// <summary>
        /// ليستي از شي ها از کلاس هايي که اينترفيس
        /// <see cref="T:DataAccess.EntityModule.Class.Configuration.Interface.IEntityConfigurationBase" />
        /// را پياده سازي کردند را برميگرداند .
        /// </summary>
        /// <returns></returns>
        private List<IEntityConfigurationBase> CreateEntityConfigurations()
        {
            /// ظرفيت اوليه يا همان capacity براي اين ليست را به مقدار 20 تا عضو بصورت پيش فرض تعيين کرديم براي اينکه تا زماني که آيتم ها به اين مقدار نرسيدند ، -
            /// آرايه ي جديدي در List در نظر نگيرد تا اعضاشان را مجددا واردش کند تا مرتبه ي زماني o(n) را برايش صرف کند . هر چند در اين حجم کوچک ، تاثير قابل مشاهده اي ندارد .
            /// 
            /// چون بصورت پيش فرض ، مقدار capacity در List ، صفر هست و هر بار که تعداد آيتم هايي که به List ميدهيم ، از مقدار capacity بيشتر شود ، -
            /// مقدار capacity مان ، 2 برابر ميشود . يعني اولين بار 0 هست و دومين بار 1 و سومين بار 2 و چهارمين بار 4 و پنجمين بار 8 و ششمين بار 16 و ... ميشود .
            /// در هر بار هم که مقدار capacity تغيير ميکند ، همه ي آيتم هاي آرايه ي جاري در ليست را به آرايه ي جديدي منتقل ميکند که مرتبه ي زماني o(n) را در پي دارد .
            int listCapacity = 20;
            List<IEntityConfigurationBase> entityConfigurations = new List<IEntityConfigurationBase>(listCapacity);

            Type iEntityConfigurationType = typeof(IEntityConfigurationBase);
            /// متغيير entityConfigurationTypes ، انواع کلاس هايي که اينترفيس IEntityConfigurationBase را پياده سازي ميکند را ذخيره ميکند .
            List<Type> entityConfigurationTypes =
                TypeUtility.GetTypesImplementingInterface(iEntityConfigurationType, iEntityConfigurationType);
            if (entityConfigurationTypes == null || entityConfigurationTypes.Count < 1)
                return null;

            foreach (Type oneEntityConfigurationType in entityConfigurationTypes)
            {
                /// شي اي از نوع کلاس هايي که اينترفيس IEntityConfigurationBase را پياده سازي ميکند را ذخيره ميکند .
                IEntityConfigurationBase entityConfigurationObject =
                    Activator.CreateInstance(oneEntityConfigurationType) as IEntityConfigurationBase;

                if (entityConfigurationObject != null)
                    entityConfigurations.Add(entityConfigurationObject);
            }

            if (entityConfigurations.Count < 0)
                return null;

            return entityConfigurations;
        }




        /// <summary>
        /// DbSet اي از موجوديت Person<br/><br/>
        /// براي استفاده از قابليت lazy loading يا بارگذاري تنبل ، يعني زماني اطلاعات از ديتابيس لود بشود که درخواست و دسترسي اي به آن انجام شده باشد ، از virtual استفاده شد .
        /// </summary>
        public virtual DbSet<PersonEntity> PersonEntities { get; set; }


        /// <summary>
        /// DbSet اي از موجوديت Province يا استان<br/><br/>
        /// چون Navigation Property ئه مربوط به اين موجوديت ، درون هيچ موجوديت ديگه نبود ،
        /// پس براي ايجاد کلاس مربوط به اين موجوديت ، شيِ DbSet اي از اين موجوديت را در اين کلاس قرار داديم .
        /// همچنين بخاطر lazyloading ، بصورت virtual تعريف شد .
        /// </summary>
        public virtual DbSet<ProvinceEntity> ProvinceEntities { get; set; }
    }

}