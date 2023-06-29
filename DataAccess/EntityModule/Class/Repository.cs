using DataAccess.EntityModule.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.ExceptionModule;
using DataAccess.ReflectionModule;
using System.Reflection;
using System.Data.Entity.Infrastructure;
using System.Runtime.Remoting.Messaging;
using System.Data.Entity.Migrations;
using DataAccess.EntityModule.Class.Entity;

namespace DataAccess.EntityModule.Class
{
    public class Repository<TNonQueryEntity> : IRepositoryAggregate<TNonQueryEntity> where TNonQueryEntity : class
    {
        private readonly DbContext _context;


        private readonly DbSet<TNonQueryEntity> _entities;




        public Repository(DbContext context)
        {
            context = context ?? throw new ArgumentNullException(nameof(context), ExceptionMessage.argumentNullExceptionMessage);

            this._context = context;
            this._entities = context.Set<TNonQueryEntity>();
        }




        /// <summary>
        /// کارهای اولیه سازی را برای پایگاه داده در
        /// <see cref="T:System.Threading.Tasks.Task" />
        /// و نخ جدیدی انجام میدهد .<br/>
        /// کارهای اولیه سازی مانند ایجاد پایگاه داده جدید یا ساخته شدن Mapping View برای پایگاه داده .
        /// </summary>
        /// <param name="force">
        /// مشخص میکند که آیا حتی اگر قبلا هم یکبار این متد اجرا شد و کارهای اولیه سازی پایگاه داده انجام شد ، آیا مجددا هم اجرا شود یا نه .<br/>
        /// زمانی مقدار true بدهید که در حین اینکه برنامه تان در حال اجرا هست و قبلا این متد را فراخوانی کرده بودید ، اما بعد از آن ، فرضا پایگاه داده تان حذف شد و میخواهید این متد را
        /// برای ایجاد کردنِ مجدد پایگاه داده ، فراخوانی کنید یا سناریوهایی از این دست .
        /// </param>
        /// <returns></returns>
        public async Task InitializeDatabaseAsync(bool force = false)
        {
            /// چون عملیات Initialize ، یک عملیات طولانی مدت هست و حتی فقط اگر بخواهد ساخت Mapping View را هم انجام دهد ، ممکن است که در حد ثانیه ،
            /// یا در حد چند ثانیه طول بکشد ، چه برسد به زمانی که بخواهد پایگاه داده ای را ایجاد کند که بسیار بیشتر طول میکشد ، بنابراین از متد Task.Factory.StartNew
            /// استفاده کردیم تا بتوانیم با مقدار TaskCreationOptions.LongRunning ، طولانی مدت بودن عملیات را اعلام و مشخص کنیم .
            ///
            /// اگر مقدار TaskCreationOptions.LongRunning برای این متد مشخص شود ، زمان بند وظیفه ، ممکن است اشاره کند که برای Task ،
            ///  یک نخ اضافی تر ممکن است لازم باشد تا فرآیند پیشرفت نخ های دیگر در صف thread-pool محلی را مسدود یا کندتر نکند .
            /// 
            /// البته هر چند در زمان ساخت دیتابیس ، چون چندان وابسته به محاسبات پردازنده نیست ، تفاوت خاصی نکند اما در زمان ساخت Mapping View چون وابسته به محاسبات نسبی
            /// در پردازنده هست ، در سیستم های با پردازنده با تعداد هسته ی خیلی کم ، ممکن است که تنظیم کردن TaskCreationOptions.LongRunning ، تفاوتی را
            /// در عملکرد و کارایی برنامه ، ایجاد کند .
            await Task.Factory.StartNew(() => this._context.Database.Initialize(force), TaskCreationOptions.LongRunning);
        }


        public IEnumerable<TQueryEntity> GetAllByLazyLoadingMode<TQueryEntity>() where TQueryEntity : class
        {
            DbSet<TQueryEntity> allRecordsQueryEntities = this._context.Set<TQueryEntity>();
            return allRecordsQueryEntities?.ToList();
        }


        public IEnumerable<TQueryEntity> GetAllByEagerLoadingMode<TQueryEntity>() where TQueryEntity : class
        {
            IList<PropertyInfo> entityVirtualProperties = TypeUtility.GetVirtualPropertiesFromType(typeof(TQueryEntity));

            /// خروجی متد GetVirtualPropertiesFromType ، ممکن هست که null نباشد و بجایش ، آیتم های آن لیست ، صفر باشند ،
            /// پس شرط را برای تعداد آیتم هایش هم گذاشتیم .
            if (entityVirtualProperties == null || entityVirtualProperties.Count < 1)
                return this.GetAllByLazyLoadingMode<TQueryEntity>();

            DbSet<TQueryEntity> allRecordsQueryEntities = this._context.Set<TQueryEntity>();
            if (allRecordsQueryEntities == null)
                return null;

            return this.GetEagerLoadedFromProperties(allRecordsQueryEntities, entityVirtualProperties)?.ToList();
        }


        private IQueryable<TQueryEntity> GetEagerLoadedFromProperties<TQueryEntity>(
        IQueryable<TQueryEntity> eagerLoadingQueryable, ICollection<PropertyInfo> entityVirtualProperties) where TQueryEntity : class
        {
            eagerLoadingQueryable = eagerLoadingQueryable ?? throw new ArgumentNullException(
                nameof(eagerLoadingQueryable), ExceptionMessage.argumentNullExceptionMessage);
            entityVirtualProperties = entityVirtualProperties ?? throw new ArgumentNullException(
                nameof(entityVirtualProperties), ExceptionMessage.argumentNullExceptionMessage);

            foreach (PropertyInfo entityVirtualProperty in entityVirtualProperties)
            {
                /// متد Include ، هر بار که اجرا شود ، شی جدیدی از کلاسی که اینترفیس IQueryable<T> را پیاده سازی کند را به همراه تغییراتی که در شی جاری اش داشت ، برمیگرداند .
                /// پس باید نتیجه اش را درون همان متغییر ذخیره کنیم . که در اینجا ، این متغییر ، همان متغییرِ eagerLoadingQueryable هست .
                eagerLoadingQueryable = eagerLoadingQueryable.Include(entityVirtualProperty.Name);
            }

            return eagerLoadingQueryable; ;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idPropertyName">
        /// نام پروپرتی ای که به عنوان شناسه ی موجودیت استفاده میشود .<br/>
        /// این مقدار میتواند null ارسال شود و در این صورت ، موجودیت مورد نظر ، بصورت Lazy Loding بارگذاری میشود .<br/>
        /// اما اگر مقدار این پارامتر ارسال شود ، موجودیت مورد نظر ، بصورت Eager Loading بارگذاری میشود که در اکثر موارد ، این حالت از بارگذاری ، مورد استفاده قرار میگیرد .
        /// </param>
        /// <returns></returns>
        public TQueryEntity FindByLazyLoadingMode<TQueryEntity>(int id) where TQueryEntity : class
        {
            DbSet<TQueryEntity> findEntities = this._context.Set<TQueryEntity>();
            return findEntities?.Find(id);
        }


        public TQueryEntity FindByEagerLoadingMode<TQueryEntity>(int id, string idPropertyName = "Id") where TQueryEntity : class
        {
            idPropertyName = idPropertyName ?? throw new ArgumentNullException(nameof(idPropertyName),
                ExceptionMessage.argumentNullExceptionMessage);

            /// در خط های پایین تر اگر مقدار متغییر entityVirtualProperties برابر با null یا تعداد اعضایش صفر باشد
            /// یا متغییر eagerLoadedQueryable هم null باشد ، به صورت  Lazy Loading بارگذاری میشود .
            /// 
            /// خط زیر را با آنکه در هر 2 خط کد ، یک متد را فراخوانی میکنند (متد FindByLazyLoadingMode را فراخوانی میکنند) ، بصورت مجزا فراخوانی کردیم -
            /// چون در صورت null بودن هر کدام از خروجی های متدها ، سربار اضافی و بی خودی برای اجرای متد GetVirtualPropertiesFromType و ... ، انجام نشود .
            ///
            /// خروجی متد GetVirtualPropertiesFromType ، ممکن هست که null نباشد و بجایش ، آیتم های آن لیست ، صفر باشند ،
            /// پس شرط را برای تعداد آیتم هایش هم گذاشتیم .
            IList<PropertyInfo> entityVirtualProperties = TypeUtility.GetVirtualPropertiesFromType(typeof(TQueryEntity));
            if (entityVirtualProperties == null || entityVirtualProperties.Count < 1)
                return this.FindByLazyLoadingMode<TQueryEntity>(id);

            DbSet<TQueryEntity> findEntities = this._context.Set<TQueryEntity>();
            if (findEntities == null)
                return null;

            IQueryable<TQueryEntity> eagerLoadedQueryable = this.GetEagerLoadedFromProperties(findEntities, entityVirtualProperties);
            if (eagerLoadedQueryable == null)
                return this.FindByLazyLoadingMode<TQueryEntity>(id);

            return this.FindByEagerLoading(id, idPropertyName, eagerLoadedQueryable);
        }


        private TQueryEntity FindByEagerLoading<TQueryEntity>(int id, string idPropertyName, IQueryable<TQueryEntity> entityQueryable)
        {
            idPropertyName = idPropertyName ?? throw new ArgumentNullException(nameof(idPropertyName),
                ExceptionMessage.argumentNullExceptionMessage);
            entityQueryable = entityQueryable ?? throw new ArgumentNullException(nameof(entityQueryable),
                ExceptionMessage.argumentNullExceptionMessage);

            return entityQueryable.FirstOrDefault(new Func<TQueryEntity, bool>(
                (TQueryEntity entityForFind) =>
                {
                    object idPropertyValue = TypeUtility.GetPropertyValueFromType(entityForFind.GetType(), idPropertyName, entityForFind);
                    return idPropertyValue?.ToString() == id.ToString();
                }
                ));
        }


        public async Task<IEnumerable<TQueryEntity>> GetAllByEagerLoadingModeAsync<TQueryEntity>() where TQueryEntity : class
        {
            return await Task.Factory.StartNew(this.GetAllByEagerLoadingMode<TQueryEntity>);
        }


        public TNonQueryEntity Add(TNonQueryEntity entity)
        {
            entity = entity ?? throw new ArgumentNullException(nameof(entity), ExceptionMessage.argumentNullExceptionMessage);

            return this._entities.Add(entity);
        }


        public void Edit(int entityIdInDatabase, TNonQueryEntity updateEntity)
        {
            updateEntity = updateEntity ?? throw new ArgumentNullException(nameof(updateEntity),
                ExceptionMessage.argumentNullExceptionMessage);

            TNonQueryEntity entityInDatabase = this.FindByLazyLoadingMode<TNonQueryEntity>(entityIdInDatabase);
            if (entityInDatabase == null)
                return;

            DbEntityEntry<TNonQueryEntity> nonQueryEntityEntry = this._context.Entry(entityInDatabase);
            DbPropertyValues nonQueryEntityPropertyValues = nonQueryEntityEntry?.CurrentValues;
            try
            {
                nonQueryEntityPropertyValues?.SetValues(updateEntity);
            }
            catch (InvalidOperationException operationException)
            {
                string checkMessage = "is part of the object's key information and cannot be modified";
                if (operationException.Message.Contains(checkMessage) == true)
                {
                    string newExceptionMessage = "مقدار پروپرتیِ مربوط به شناسه ، باید برابر با همان مقدار قبلی و یا در واقع ، برابر با همان مقداری " +
                        "که به عنوان پارامتر اول به این متد ارسال شد (برابر با پارامتر entityIdInDatabase) ، باشد و نمیتواند تغییر داده شود .";

                    throw new InvalidOperationException(newExceptionMessage, operationException);
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idPropertyName">
        /// نام پروپرتی ای که به عنوان شناسه ی موجودیت استفاده میشود .<br/>
        /// این مقدار میتواند null ارسال شود و در این صورت ، موجودیت مورد نظر ، بصورت Lazy Loding بارگذاری میشود .<br/>
        /// اما اگر مقدار این پارامتر ارسال شود ، موجودیت مورد نظر ، بصورت Eager Loading بارگذاری میشود
        /// تا به عنوان موجودیتی که قابل حذف باشد ، نشانه گذاری شود . که در اکثر موارد ، این حالت از بارگذاری ، مورد استفاده قرار میگیرد .
        /// </param>
        public TRemoveEntity Remove<TRemoveEntity>(TRemoveEntity removeEntity) where TRemoveEntity : class
        {
            removeEntity = removeEntity ?? throw new ArgumentNullException(nameof(removeEntity), 
                ExceptionMessage.argumentNullExceptionMessage);

            return this._context.Set<TRemoveEntity>()?.Remove(removeEntity);
        }


        /// <summary>
        /// ذخیره ی تغییرات را انجام میدهد .<br/><br/>
        /// به منظور انجام الگوی Unit Of Work برای اینکه چندین تغییرات و تراکنش را بشود یکجا انجام داد ، این متد ، بصورت مجزا ارائه شد .
        /// </summary>
        /// <returns>
        /// تعداد مدخل های نوشته شده به پایگاه داده را برمیگرداند .
        /// </returns>
        public int SaveChanges()
        {
            int savedStateNumber = 0;
            try
            {
                savedStateNumber = this._context.SaveChanges();
            }
            catch (DbUpdateException dbUpdateException)
            {
                this.MobileNumberUniqueExceptionHandler(dbUpdateException);
            }
            return savedStateNumber;
        }


        /// <summary>
        /// ذخیره ی تغییرات را بصورت ناهمگام انجام میدهد .<br/><br/>
        /// به منظور انجام الگوی Unit Of Work برای اینکه چندین تغییرات و تراکنش را بشود یکجا انجام داد ، این متد ، بصورت مجزا ارائه شد .
        /// </summary>
        /// <returns>
        /// تعداد مدخل های نوشته شده به پایگاه داده را برمیگرداند .
        /// </returns>
        public async Task<int> SaveChangesAsync()
        {
            int savedStateNumber = 0;
            try
            {
                savedStateNumber = await this._context.SaveChangesAsync();
            }
            catch (DbUpdateException dbUpdateException)
            {
                this.MobileNumberUniqueExceptionHandler(dbUpdateException);
            }

            return savedStateNumber;
        }


        private void MobileNumberUniqueExceptionHandler(DbUpdateException dbUpdateException)
        {
            string message = dbUpdateException?.InnerException?.InnerException?.Message;
            if (message != null)
            {
                string showMessage = "نمیتوانید دو یا چند شماره ی همراه تکراری وارد کنید . لطفا شماره همراه منحصر به فرد وارد کنید .";
                string containMessage = "Cannot insert duplicate key row in object 'dbo.MobileNumberTable' with unique index 'MobileNumber_UniqueIndex'";
                if (message.Contains(containMessage) == true)
                    throw new Exception(showMessage, dbUpdateException);
            }

            throw dbUpdateException;
        }


        public void Dispose()
        {
            this._context.Dispose();
        }


    }
}
