using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.EntityModule.Class;
using DataAccess.EntityModule.Class.Entity;
using DataAccess.EntityModule.Class.PreGeneratedView;

namespace DataAccess
{
    public class TestInDataAccess
    {
        //public long Fibonacci(object obj)
        //{
        //    if (!(obj is int))
        //    {
        //        throw new ArgumentException("obj must be of type int");
        //    }

        //    return 0;
        //}


        public async Task Test()
        {
            using (PhoneBookDbContext context = new PhoneBookDbContext())
            {
                await context.InitializeDatabaseAsync();
            }

            //List<MobileNumberEntity> mobileNums = new List<MobileNumberEntity>();
            //mobileNums.Add(new MobileNumberEntity() { MobileNumber = "09455456" });
            //mobileNums.Add(new MobileNumberEntity() { MobileNumber = "9875412" });


            //context.PersonEntities.Add(new EntityModule.Class.Entity.PersonEntity()
            //    { Id = 1, FirstName = "abc", LastName = "cde", MobileNumbers = mobileNums });

            //context.SaveChanges();
        }


    }

}
