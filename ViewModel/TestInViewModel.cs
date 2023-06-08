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
            //person.Id = 1;
            //person.FirstName = "غلام حسین";
            //person.LastName = "افشردی";
            //person.Mobiles.Add(new Mobile() {MobileNumber = "45"});
            //Address address = new Address() {Province = "تهران", City = "تهران", AddressDetail = "اسلام شهر"};
            //address.Persons.Add(person);
            //person.Addresses.Add(address);

            IPhoneBookAggregator phoneBook = new PhoneBook();
            await phoneBook.LoadPeopleAsync();

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
