using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.ExceptionModule;

namespace DataAccess.ReflectionModule
{
    /// <summary>
    /// این کلاس ، شامل اعضایی که با نوع یا Type ها کار میکند .
    /// متدهایی که نوع کلاس هایی که نوع خاصی از اینترفیس را پیاده سازی کردند را برمیگرداند یا از نوع خاصی شی میسازد که برای کار در زمان اجرا مفیدند .
    /// </summary>
    public class TypeUtility
    {
        /// <summary>
        /// انواع کلاس هایی را برمیگرداند که در یک اسبلی (پروژه و دی ال ال) ، یک اینترفیس خاصی را پیاده سازی میکنند <br/>
        /// توضیحات بیشتر : <br/>
        /// معمولا نوع هر دو پارامتر های این متد ، یعنی نوع typeInsideAssembly و interfaceType ، یک نوع هستند مگر در مواقع مورد نیاز .
        /// </summary>
        /// <param name="typeInsideAssembly">
        ///  نوعی که درون یک اسمبلی یا پروژه یا dll قرار دارد تا کلاس های آن اسمبلی بررسی شود که آیا آن کلاس ها ، نوع اینترفیسِ interfaceType را پیاده سازی کردند یا نه .
        /// </param>
        /// <param name="interfaceType">
        ///  نوع اینترفیس ای که مشخص شود آیا کلاس یا کلاس هایی درونِ اسمبلی ای که در نوع typeInsideAssembly قرار دارند ، آن کلاس ها ، این اینترفیس را پیاده سازی کردند یا نه .
        /// </param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        ///  هیچ کدام از پارامترهای این متد نباید null ارسال شوند وگرنه این استثناء پرتاب میشود .
        /// </exception>
        public static IList<Type> GetTypesImplementingInterface(Type typeInsideAssembly, Type interfaceType)
        {
            /// اگر پارامتر ارسالیِ typeInsideAssembly ، مقدار null ارسال شود ، سمت راست عملگر ?? که استثنای ArgumentNullException هست ، پرتاب میشود و
            /// برنامه متوقف میشود وگرنه مقدار این متغییر ، مجددا درون خود همان متغییر ریخته میشود .
            /// بجای دستور if ، از عملگر ?? استفاده شده .
            typeInsideAssembly = typeInsideAssembly ?? throw new ArgumentNullException(nameof(typeInsideAssembly),
                ExceptionMessage.argumentNullExceptionMessage);
            interfaceType = interfaceType ?? throw new ArgumentNullException(nameof(interfaceType),
                ExceptionMessage.argumentNullExceptionMessage);

            /// اسمبلیِ (شامل همه ی مشخصات کلاس ها و اینترفیس ها و استراکچر و اینام هایی که در پروژه) ای که آن نوعِ داده شده در آن تعریف شد را برمیگرداند .
            /// 
            /// مثلا اگر نوع typeInsideAssembly ، از نوع اینترفیسِ IEntityConfigurationAggregate باشد ، اسمبلیِ مربوط به پروژه و فایل dll ای-
            /// که اینترفیسِ IEntityConfigurationAggregate درونش تعریف شد ، یعنی همه ی اطلاعات مربوط به کلاس ها و اینترفیس ها و ... ای که در پروژه و لایه ی DataAccess و -
            /// و در فایل DataAccess.dll تعریف شد را برمیگرداند .
            Assembly typeAssembly = typeInsideAssembly.Assembly;
            /// همه ی انواع (شامل کلاس و اینترفیس و اینام و استراکچر و ...) را برمیگرداند .
            Type[] allTypesInsideAssembly = typeAssembly?.GetTypes();
            /// متد در پارامتر Where ، بررسی میکند که اگر در انواع پیدا شده در اسمبلی typeAssembly ، اگر انواعی از کلاسی پیدا شد که نوع اینترفیسِ interfaceType را -
            /// پیاده سازی میکند ، آن انواع را برمیگرداند .
            ///
            /// دقت کنید که خروجی Where چون IEnumerable هست ، متدی که درون پارامترش بصورت بی نام تعریف شده ، لحظه ی فراخوانیِ متدِ Where ، اجرا نمیشود -
            /// بلکه معمولا زمانی اجرا میشود که بخواهیم روی خروجیِ آیتم های اش که IEnumerable بود ، بخواهیم iterate کنیم و فرضا در حلقه از آن استفاده کنیم .
            /// به این دلیل و همچنین به دلیل اینکه  IEnumerable ، فقط یک بار قابل iterate شدن هست و برای دفعات بعد ، خطا و استثناء پرتاب میکند ، فورا آن را به نوع List تبدیل کردیم .
            List<Type> typesImplementingInterface = allTypesInsideAssembly?.Where(
                (Type assemblyOneType) =>
                {
                    /// متغییر assemblyOneType ، در هر بار تکرارِ این متد ، شامل یک نوع از این اسمبلی میشود . مثلا نوع کلاس یا اینترفیس و ... .
                    /// متغییر allInterfacesTypes ، اگر یک نوع ، اینترفیس ای را پیاده سازی کند ، یا اجدادشان ، اینترفیسی را پیاده سازی کنند ، نوع تمام آن اینترفیس ها را برمیگردانند .
                    Type[] allInterfacesTypes = assemblyOneType.GetInterfaces();
                    /// متغییر hasAssemblyOneTypeImplementingInterfaceType ، مشخص میکند که آیا در لیستِ انواعِ اینترفیس هایی که در متغییر allInterfacesTypes -
                    /// پیدا شد ، آیا نوع اینترفیسی که در پارامتر دوم متد _ یعنی پارامتر interfaceType _ یافت شد یا نه؟
                    /// 
                    /// دقت شود که چون از عملگر ?. در متد زیر استفاده کردیم و این عملگر ، اگر عبارت سمت چپ اش که متغییر allInterfacesTypes هست ، مقدار null باشد ، پس در -
                    /// این صورت ، مقدار خروجی اش و در نتیجه اش ، مقدار null را ذخیره میکند ، پس نوع متغییر خروجی در زیر را از نوع بولین ای که قابل Nulable هست ، گرفتیم .
                    bool? hasAssemblyOneTypeImplementingInterfaceType = allInterfacesTypes?.Contains(interfaceType);
                    return hasAssemblyOneTypeImplementingInterfaceType.Value;
                })?.ToList();

            return typesImplementingInterface;
        }



        public static IList<PropertyInfo> GetVirtualPropertiesFromType(Type type)
        {
            type = type ?? throw new ArgumentNullException(nameof(type), ExceptionMessage.argumentNullExceptionMessage);

            PropertyInfo[] allPropertiesInsideType = type.GetProperties();
            List<PropertyInfo> virtualPropertiesInsideType = allPropertiesInsideType?.Where((PropertyInfo propertyInfo) =>
            {
                bool? isVirtualProperty = propertyInfo.GetMethod?.IsVirtual;
                return (isVirtualProperty != null) ? isVirtualProperty.Value : false;
            }).ToList();

            return virtualPropertiesInsideType;
        }


        public static object GetPropertyValueFromType(Type type, string publicPropertyName, object instance)
        {
            type = type ?? throw new ArgumentNullException(nameof (type), ExceptionMessage.argumentNullExceptionMessage);
            publicPropertyName = publicPropertyName ?? throw new ArgumentNullException(nameof(publicPropertyName), 
                ExceptionMessage.argumentNullExceptionMessage);
            instance = instance ?? throw new ArgumentNullException(nameof(instance), ExceptionMessage.argumentNullExceptionMessage);

            return type.GetProperty(publicPropertyName)?.GetValue(instance);
        }


    }
}
