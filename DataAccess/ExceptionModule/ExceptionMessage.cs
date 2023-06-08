using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ExceptionModule
{
    /// <summary>
    /// کلاسی استاتیک برای متن آماده برای پیغام های Exception
    /// </summary>
    public static class ExceptionMessage
    {
        /// <summary>
        /// متن پیغام برای زمانی که ورودی و آرگومانِ متدی ، null ارسال میشود .
        /// </summary>
        public static readonly string argumentNullExceptionMessage = "این پارامتر نباید null باشد .";


        public static readonly string entityNotFoundExceptionMessage = "موجودیت مورد نظر ، یافت نشد";
    }
}
