﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityModule.Class.Configuration.Interface
{
    /// <summary>
    ///  پیکربندی پایه دیتابیس را انجام میدهد .<br/>
    /// کلاس هایی این اینترفیس را پیاده سازی میکنند که کلاس های مربوط به موجودیت هایی را ، به ازایش دارند ، و پیکربندی های آن موجودیت را در آن کلاس انجام میدهند .<br/>
    /// این ، یک اینترفیس پایه هست و شامل اعضایی برای پیکربندی دیتابیس هست که برای همه ی کلاس های موجودیت ها باید بکار برده شوند . <br/><br/>
    /// 
    /// این اینترفیس ، برای پیاده سازی اصل Dependency Inversion در اصول Solid ، اجرا نشد . چون کلاس
    /// <see cref="T:DataAccess.EntityModule.Class.PhoneBookDbContext" />
    /// از این اینترفیس استفاده میکند و همه آن کلاس ، و هم کلاس هایی که این اینترفیس را پیاده سازی میکنند ، در یک ماژول هستند و بنابراین ماژول و کلاس سطح بالا و پایین در اینجا
    /// معنا ندارد که بخواهیم این اینترفیس را برای این اصل در اینجا پیاده سازی کنیم . چون ارتباطات درون یک ماژول ، بیشترین وابستگی به همدیگر را دارند .<br/><br/>
    ///
    /// این اینترفیس ، برای بالا بردن یکپارچگی کلاس های مختلفی که پیکربندی را انجام میدهند است تا متدهای هم نامی داشته باشند و مخصوصا اینکه برای شی ساختن از این کلاس ها
    /// در زمان اجرای برنامه ، بتوانیم به تمام این کلاس هایی که یک اینترفیس خاصی (این اینترفیس) را پیاده سازی میکنند ، دسترسی داشته باشیم .
    /// </summary>
    internal interface IEntityConfigurationBase : ITableConfiguration, IAttributeConfigurationBase
    {
    }
}