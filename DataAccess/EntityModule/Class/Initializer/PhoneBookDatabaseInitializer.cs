using DataAccess.EntityModule.Class.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using DataAccess.ExceptionModule;
using System.IO;
using DataAccess.EntityModule.Class.PreGeneratedView;
using DataAccess.SerializationModule;

namespace DataAccess.EntityModule.Class.Initializer
{
    /// <summary>
    /// این کلاس برای اولیه سازی و ساخت دیتابیس جدید فقط در صورتی که اگر آن دیتابیس ، از قبل وجود نداشته باشد ، بکار میرود .
    /// </summary>
    internal class PhoneBookDatabaseInitializer : CreateDatabaseIfNotExists<PhoneBookDbContext>
    {
        /// <summary>
        /// اگر دیتابیس مان وجود نداشته باشد ، این متد اجرا میشود . یعنی فقط برای یکبار اجرا میشود .<br/>
        /// در این متد ، اطلاعات Mapping View ، در فایل xml ای بنام PreGeneratedMappingViews.xml در مسیر پوشه ی فایل اجرایی برنامه ، ساخته میشود .<br/>
        /// همچنین سطرهای جدول ها و موجودیت های استان و شهر ، برای یکبار مقداردهی میشوند .
        /// </summary>
        /// <param name="context">
        /// شیِ
        /// <see cref="T:DataAccess.EntityModule.Class.PhoneBookDbContext" />
        /// </param>
        /// <exception cref="ArgumentNullException"></exception>
        protected override void Seed(PhoneBookDbContext context)
        {
            context = context ?? throw new ArgumentNullException(nameof(context), ExceptionMessage.argumentNullExceptionMessage);

            this.SerializePreGeneratedDbMappingViewInfo(context);
            this.SetProvinceCityEntity(context);

            base.Seed(context);
        }


        /// <summary>
        /// در این متد ، اطلاعات Mapping View ، در فایل xml ای بنام PreGeneratedMappingViews.xml در مسیر پوشه ی فایل اجرایی برنامه ،  serialize و ذخیره میشود .<br/>
        /// هر چند این متد به درستی کارش انجام میشود اما در کلاس PhoneBookDbMappingViewCache ، نمیدانم به چه علت ، فقط یکبار آن هم زمان ساخت فایل ،
        /// اعضایش فراخوانی میشوند و در دفعات اجرای بعدیِ برنامه ، اعضایش فراخوانی نمیشوند ؛ با آنکه اتریبیوتِ زیر را در فایل assemblyinfo.cs در این پروژه استفاده کردیم :
        /// [assembly: DbMappingViewCacheType(typeof(PhoneBookDbContext), typeof(PhoneBookDbMappingViewCache))]
        /// بنابراین خواندن و استفاده از اطلاعات  Mapping View فعلا در این برنامه استفاده نمیشوند تا به این علت ، سرعت اجرای دفعه ی اول برنامه در دستور EF ، بیشتر شود .
        /// </summary>
        /// <param name="context"></param>
        private void SerializePreGeneratedDbMappingViewInfo(DbContext context)
        {
            string xmlFullFileName = "PreGeneratedMappingViews.xml";
            string fullFilePath = AppDomain.CurrentDomain.BaseDirectory + xmlFullFileName;
            if (File.Exists(fullFilePath) == true)
                return;

            DbMappingViewInfo dbMappingViewInfo = DbMappingViewGenerator.GetDbMappingViewInfo(context);
            if (dbMappingViewInfo == null) 
                return;

            XmlDataSerializer.WriteDataToXmlFile(dbMappingViewInfo, xmlFullFileName);
        }


        /// <summary>
        /// مقداردهی اولیه ی سطرهای جدول و موجودیت استان و شهر را انجام میدهد .
        /// </summary>
        /// <param name="context"></param>
        private void SetProvinceCityEntity(PhoneBookDbContext context)
        {
            IEnumerable<ProvinceEntity> provinceEntities = new List<ProvinceEntity>()
            {
                new ProvinceEntity(){ProvinceName="آذربایجان شرقی",
                    CityEntities=new List<CityEntity>()
                    {
                        new CityEntity(){CityName = "تبریز"},
                        new CityEntity(){CityName = "بناب"},
                        new CityEntity(){CityName = "اهر"},
                        new CityEntity(){CityName = "مرند"},
                        new CityEntity(){CityName = "مراغه"},
                        new CityEntity(){CityName = "عجب شیر"},
                    }},
                new ProvinceEntity(){ProvinceName="آذربایجان غربی",
                    CityEntities=new List<CityEntity>()
                    {
                        new CityEntity(){CityName = "ارومیه"},
                        new CityEntity(){CityName = "خوی"},
                        new CityEntity(){CityName = "نقده"},
                        new CityEntity(){CityName = "پیرانشهر"},
                        new CityEntity(){CityName = "سردشت"},
                        new CityEntity(){CityName = "میاندوآب"},
                    }},
                new ProvinceEntity(){ProvinceName="اردبیل",
                    CityEntities=new List<CityEntity>()
                    {
                        new CityEntity(){CityName = "اردبیل"},
                        new CityEntity(){CityName = "اسلام اباد"},
                        new CityEntity(){CityName = "مشگین شهر"},
                        new CityEntity(){CityName = "خلخال"},
                    }},
                new ProvinceEntity(){ProvinceName="اصفهان",
                    CityEntities=new List<CityEntity>()
                    {
                        new CityEntity(){CityName = "کاشان"},
                        new CityEntity(){CityName = "اردستان"},
                        new CityEntity(){CityName = "میمه"},
                        new CityEntity(){CityName = "آران و بیدگل"},
                        new CityEntity(){CityName = "نجف آباد"},
                        new CityEntity(){CityName = "زاینده رود"},
                        new CityEntity(){CityName = "شاهین شهر"},
                        new CityEntity(){CityName = "گلپایگان"},
                        new CityEntity(){CityName = "اصفهان"},
                        new CityEntity(){CityName = "نطنز"},
                    }},
                new ProvinceEntity(){ProvinceName="البرز",
                    CityEntities=new List<CityEntity>()
                    {
                        new CityEntity(){CityName = "کرج"},
                        new CityEntity(){CityName = "طالقان"},
                    }},
                new ProvinceEntity(){ProvinceName="ایلام",
                    CityEntities=new List<CityEntity>()
                    {
                        new CityEntity(){CityName = "ایلام"},
                        new CityEntity(){CityName = "مهران"},
                        new CityEntity(){CityName = "دهلران"},
                    }},
                new ProvinceEntity(){ProvinceName="بوشهر",
                    CityEntities=new List<CityEntity>()
                    {
                        new CityEntity(){CityName = "براز جان"},
                        new CityEntity(){CityName = "بندر گناوه"},
                        new CityEntity(){CityName = "خارک"},
                        new CityEntity(){CityName = "دلوار"},
                        new CityEntity(){CityName = "عسلویه"},
                        new CityEntity(){CityName = "بوشهر"},
                    }},
                new ProvinceEntity(){ProvinceName="تهران",
                    CityEntities=new List<CityEntity>()
                    {
                        new CityEntity(){CityName = "ری"},
                        new CityEntity(){CityName = "رودهن"},
                        new CityEntity(){CityName = "قرچک"},
                        new CityEntity(){CityName = "ورامین"},
                        new CityEntity(){CityName = "فیروزکوه"},
                        new CityEntity(){CityName = "آبعلی"},
                        new CityEntity(){CityName = "تهران"},
                        new CityEntity(){CityName = "بومهن"},
                        new CityEntity(){CityName = "کهریزک"},
                        new CityEntity(){CityName = "رباط کریم"},
                        new CityEntity(){CityName = "تجریش"},
                        new CityEntity(){CityName = "اسلامشهر"},
                        new CityEntity(){CityName = "دماوند"},
                        new CityEntity(){CityName = "پردیس"},
                    }},
                new ProvinceEntity(){ProvinceName="چهارمحال و بختیاری",
                    CityEntities=new List<CityEntity>()
                    {
                        new CityEntity(){CityName = "شهرکرد"},
                        new CityEntity(){CityName = "طاقانک"},
                        new CityEntity(){CityName = "بابا حیدر"},
                    }},
                new ProvinceEntity(){ProvinceName="خراسان جنوبی",
                    CityEntities=new List<CityEntity>()
                    {
                        new CityEntity(){CityName = "بیرجند"},
                        new CityEntity(){CityName = "فردوس"},
                        new CityEntity(){CityName = "طبس"},
                    }},
                new ProvinceEntity(){ProvinceName="خراسان رضوی",
                    CityEntities=new List<CityEntity>()
                    {
                        new CityEntity(){CityName = "چناران"},
                        new CityEntity(){CityName = "مشهد"},
                        new CityEntity(){CityName = "کاشمر"},
                        new CityEntity(){CityName = "شاندیز"},
                        new CityEntity(){CityName = "قدمگاه"},
                        new CityEntity(){CityName = "داورزن"},
                        new CityEntity(){CityName = "قوچان"},
                        new CityEntity(){CityName = "نیشابور"},
                        new CityEntity(){CityName = "رضویه"},
                        new CityEntity(){CityName = "تربت حیدریه"},
                        new CityEntity(){CityName = "سبزوار"},
                        new CityEntity(){CityName = "تربت جام"},
                        new CityEntity(){CityName = "طرقبه"},
                        new CityEntity(){CityName = "خلیل آباد"},
                    }},
                new ProvinceEntity(){ProvinceName="خراسان شمالی",
                    CityEntities=new List<CityEntity>()
                    {
                        new CityEntity(){CityName = "شیروان"},
                        new CityEntity(){CityName = "آشخانه"},
                        new CityEntity(){CityName = "بجنورد"},
                    }},
                new ProvinceEntity(){ProvinceName="خوزستان",
                    CityEntities=new List<CityEntity>()
                    {
                        new CityEntity(){CityName = "شاوور"},
                        new CityEntity(){CityName = "مسجد سلیمان"},
                        new CityEntity(){CityName = "دارخوین"},
                        new CityEntity(){CityName = "حمیدیه"},
                        new CityEntity(){CityName = "اندیمشک"},
                        new CityEntity(){CityName = "شادگان"},
                        new CityEntity(){CityName = "بندر ماهشهر"},
                        new CityEntity(){CityName = "بستان"},
                        new CityEntity(){CityName = "اهواز"},
                        new CityEntity(){CityName = "فتح المبین"},
                        new CityEntity(){CityName = "شوشتر"},
                        new CityEntity(){CityName = "ایذه"},
                        new CityEntity(){CityName = "هویزه"},
                        new CityEntity(){CityName = "شوش"},
                        new CityEntity(){CityName = "دزفول"},
                        new CityEntity(){CityName = "آبادان"},
                        new CityEntity(){CityName = "خرمشهر"},
                        new CityEntity(){CityName = "چمران"},
                        new CityEntity(){CityName = "امیدیه"},
                        new CityEntity(){CityName = "سوسنگرد"},
                    }},
                new ProvinceEntity(){ProvinceName="زنجان",
                    CityEntities=new List<CityEntity>()
                    {
                        new CityEntity(){CityName = "ابهر"},
                        new CityEntity(){CityName = "زنجان"},
                    }},
                new ProvinceEntity(){ProvinceName="سمنان",
                    CityEntities=new List<CityEntity>()
                    {
                        new CityEntity(){CityName = "دامغان"},
                        new CityEntity(){CityName = "شاهرود"},
                        new CityEntity(){CityName = "سمنان"},
                        new CityEntity(){CityName = "گرمسار"},
                        new CityEntity(){CityName = "بسطام"},
                        new CityEntity(){CityName = "میامی"},
                        new CityEntity(){CityName = "شهمیرزاد"},
                    }},
                new ProvinceEntity(){ProvinceName="سیستان و بلوچستان",
                    CityEntities=new List<CityEntity>()
                    {
                        new CityEntity(){CityName = "زاهدان"},
                        new CityEntity(){CityName = "زابل"},
                        new CityEntity(){CityName = "چاه بهار"},
                        new CityEntity(){CityName = "سراوان"},
                        new CityEntity(){CityName = "خاش"},
                    }},
                new ProvinceEntity(){ProvinceName="فارس",
                    CityEntities=new List<CityEntity>()
                    {
                        new CityEntity(){CityName = "شیراز"},
                        new CityEntity(){CityName = "جهرم"},
                        new CityEntity(){CityName = "استهبان"},
                    }},
                new ProvinceEntity(){ProvinceName="قزوین",
                    CityEntities=new List<CityEntity>()
                    {
                        new CityEntity(){CityName = "آبگرم"},
                        new CityEntity(){CityName = "آبیک"},
                        new CityEntity(){CityName = "قزوین"},
                    }},
                new ProvinceEntity(){ProvinceName="قم",
                    CityEntities=new List<CityEntity>()
                    {
                        new CityEntity(){CityName = "قم"},
                        new CityEntity(){CityName = "سلفچگان"},
                        new CityEntity(){CityName = "دستجرد"},
                    }},
                new ProvinceEntity(){ProvinceName="کردستان",
                    CityEntities=new List<CityEntity>()
                    {
                        new CityEntity(){CityName = "سنندج"},
                        new CityEntity(){CityName = "بانه"},
                        new CityEntity(){CityName = "مریوان"},
                        new CityEntity(){CityName = "سقز"},
                    }},
                new ProvinceEntity(){ProvinceName="کرمان",
                    CityEntities=new List<CityEntity>()
                    {
                        new CityEntity(){CityName = "بم"},
                        new CityEntity(){CityName = "سیرجان"},
                        new CityEntity(){CityName = "کرمان"},
                        new CityEntity(){CityName = "رفسنجان"},
                        new CityEntity(){CityName = "جیرفت"},
                        new CityEntity(){CityName = "رابر"},
                    }},
                new ProvinceEntity(){ProvinceName="کرمانشاه",
                    CityEntities=new List<CityEntity>()
                    {
                        new CityEntity(){CityName = "قصر شیرین"},
                        new CityEntity(){CityName = "گیلان غرب"},
                        new CityEntity(){CityName = "پاوه"},
                        new CityEntity(){CityName = "کرمانشاه"},
                        new CityEntity(){CityName = "سرپل ذهاب"},
                        new CityEntity(){CityName = "باینگان"},
                        new CityEntity(){CityName = "اسلام آباد غرب"},
                        new CityEntity(){CityName = "سومار"},
                    }},
                new ProvinceEntity(){ProvinceName="کهگیلویه و بویر احمد",
                    CityEntities=new List<CityEntity>()
                    {
                        new CityEntity(){CityName = "سی سخت"},
                        new CityEntity(){CityName = "یاسوج"},
                    }},
                new ProvinceEntity(){ProvinceName="گلستان",
                    CityEntities=new List<CityEntity>()
                    {
                        new CityEntity(){CityName = "گنبد کاووس"},
                        new CityEntity(){CityName = "کردکوی"},
                        new CityEntity(){CityName = "بندر ترکمن"},
                        new CityEntity(){CityName = "آق قلا"},
                        new CityEntity(){CityName = "بندر گز"},
                        new CityEntity(){CityName = "مینودشت"},
                        new CityEntity(){CityName = "گرگان"},
                        new CityEntity(){CityName = "علی اباد"},
                        new CityEntity(){CityName = "کلاله"},
                    }},
                new ProvinceEntity(){ProvinceName="گیلان",
                    CityEntities=new List<CityEntity>()
                    {
                        new CityEntity(){CityName = "منجیل"},
                        new CityEntity(){CityName = "فومن"},
                        new CityEntity(){CityName = "بندر انزلی"},
                        new CityEntity(){CityName = "لاهیجان"},
                        new CityEntity(){CityName = "صومعه سرا"},
                        new CityEntity(){CityName = "رودبار"},
                        new CityEntity(){CityName = "رشت"},
                        new CityEntity(){CityName = "ماسوله"},
                        new CityEntity(){CityName = "گوراب زرمیخ"},
                    }},
                new ProvinceEntity(){ProvinceName="لرستان",
                    CityEntities=new List<CityEntity>()
                    {
                        new CityEntity(){CityName = "پلدختر"},
                        new CityEntity(){CityName = "بروجرد"},
                        new CityEntity(){CityName = "الیگودرز"},
                        new CityEntity(){CityName = "خرم آباد"},
                    }},
                new ProvinceEntity(){ProvinceName="مازندران",
                    CityEntities=new List<CityEntity>()
                    {
                        new CityEntity(){CityName = "چالوس"},
                        new CityEntity(){CityName = "کیاسر"},
                        new CityEntity(){CityName = "تنکابن"},
                        new CityEntity(){CityName = "رامسر"},
                        new CityEntity(){CityName = "کیاکلا"},
                        new CityEntity(){CityName = "نور"},
                        new CityEntity(){CityName = "بابل"},
                        new CityEntity(){CityName = "جویبار"},
                        new CityEntity(){CityName = "آلاشت"},
                        new CityEntity(){CityName = "آمل"},
                        new CityEntity(){CityName = "شیرود"},
                        new CityEntity(){CityName = "قائمشهر"},
                        new CityEntity(){CityName = "ساری"},
                        new CityEntity(){CityName = "چمستان"},
                    }},
                new ProvinceEntity(){ProvinceName="مرکزی",
                    CityEntities=new List<CityEntity>()
                    {
                        new CityEntity(){CityName = "تفرش"},
                        new CityEntity(){CityName = "اراک"},
                        new CityEntity(){CityName = "دلیجان"},
                        new CityEntity(){CityName = "شهباز"},
                    }},
                new ProvinceEntity(){ProvinceName="هرمزگان",
                    CityEntities=new List<CityEntity>()
                    {
                        new CityEntity(){CityName = "قشم"},
                        new CityEntity(){CityName = "کیش"},
                        new CityEntity(){CityName = "بندر عباس"},
                        new CityEntity(){CityName = "هرمز"},
                    }},
                new ProvinceEntity(){ProvinceName="همدان",
                    CityEntities=new List<CityEntity>()
                    {
                        new CityEntity(){CityName = "زنگنه"},
                        new CityEntity(){CityName = "همدان"},
                        new CityEntity(){CityName = "نهاوند"},
                    }},
                new ProvinceEntity(){ProvinceName="یزد",
                    CityEntities=new List<CityEntity>()
                    {
                        new CityEntity(){CityName = "ندوشن"},
                        new CityEntity(){CityName = "یزد"},
                        new CityEntity(){CityName = "اردکان"},
                        new CityEntity(){CityName = "بافق"},
                        new CityEntity(){CityName = "میبد"},
                    }},
            };

            context.ProvinceEntities.AddRange(provinceEntities);
            context.SaveChanges();
        }


    }
}
