using Model.PhoneBookModule.Class;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.PhoneBookModule.Interface
{
    public interface IPhoneBook
    {
        IList<Person> People { get; }


        Task LoadPeopleAsync();


        /// <summary>
        ///
        /// این متد ، شیِ جدیدی از Person را فقط در پایگاه داده اضافه میکند .<br/>
        /// یعنی این شی را درون کالکشن People اضافه نمیکند .
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        bool AddPerson(Person person);


        /// <summary>
        /// 
        /// این متد ، شیِ موجودی از Person را فقط در پایگاه داده ویرایش میکند .<br/>
        /// یعنی این شی را درون کالکشن People ویرایش نمیکند .
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        bool EditPerson(Person person);


        /// <summary>
        ///
        /// 
        /// نیازی به متد Delete نیست . متدِ حذف کردن ، توسط رویداد حذف در پروپرتی People فراخوانی میشود و گزارش اش توسط این رویداد ، اطلاع داده میشود .
        /// </summary>
        event EventHandler<DeleteStatusEventArgs> PersonDeleteStatus;
    }
}
