using Model;
using Model.PhoneBookModule.Class;
using Model.PhoneBookModule.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ViewModel
{
    public class TestInViewModel
    {
        public async Task Test()
        {
            //Person person = new Person();
            //person.Id = 4;
            //person.FirstName = "علی اکبر";
            //person.LastName = "شیرودی";
            //person.Mobiles.Add(new Mobile() { MobileNumber = "111111111" });
            //person.Mobiles.Add(new Mobile() { MobileNumber = "222222222" });
            //Address address = new Address() { Province = "ایلام", City = "ایلام"};
            //address.Persons.Add(person);
            //person.Addresses.Add(address);

            /////////

            IPhoneBookAggregator phoneBook = new PhoneBook();
            await phoneBook.LoadPeopleAsync();
            //await phoneBook.LoadProvincesAsync();
            //phoneBook.LoadCitiesByProvinceId(13);

            //person.Addresses.Add(phoneBook.People[2].Addresses[0]);
            //phoneBook.EditPerson(person);
            phoneBook.People.RemoveAt(3);
        }


        //public async Task MyTest_2()
        //{
        //    Console.WriteLine($"MyTest_2 thread  :  {Thread.CurrentThread.ManagedThreadId}");

        //    SynchronizationContext synchronizationContext = SynchronizationContext.Current;
        //    await Task.Delay(1000).ContinueWith((Task myTask) =>
        //    {
        //        synchronizationContext.Send((object param) =>
        //        {
        //            Console.WriteLine($"synchronizationContext thread  :  {Thread.CurrentThread.ManagedThreadId}");
        //        }, null);
        //    });
        //}




    }
}
