using DataAccess;
using DataAccess.EntityModule.Class;
using DataAccess.EntityModule.Class.Entity;
using DataAccess.EntityModule.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class TestInModel
    {
        public async Task Test()
        {
            using (IRepositoryBase<PersonEntity> repository = new Repository<PersonEntity>(new PhoneBookDbContext()))
            {
                await repository.InitializeDatabaseAsync();

                PersonEntity mainEntity = repository.FindByLazyLoadingMode<PersonEntity>(1);
                // ویرایش پروپرتی های scalar برای موجودیت PersonEntity
                mainEntity.FirstName = "سردار مقاومت احمد";
                mainEntity.LastName = "متوسلیان";

                mainEntity.MobileNumbers = mainEntity.MobileNumbers ?? new List<MobileNumberEntity>();
                // ویرایش ، شامل حذف و اضافه و ویرایش موجودیت MobileNumberEntity
                MobileNumberEntity addMne = new MobileNumberEntity() {MobileNumber = "0112233"};
                mainEntity.MobileNumbers.Add(addMne);

                MobileNumberEntity deleteMne = mainEntity.MobileNumbers.FirstOrDefault(mobileNumberEntity => mobileNumberEntity.Id == 7);
                mainEntity.MobileNumbers.Remove(deleteMne);
                repository.Remove<MobileNumberEntity>(deleteMne);

                mainEntity.MobileNumbers.FirstOrDefault(mobileNumberEntity => mobileNumberEntity.Id == 6).MobileNumber = "987654";

                /////////////////////////////
                // ویرایش ، شامل حذف و اضافه و ویرایش موجودیت AddressEntity
                mainEntity.Addresses = mainEntity.Addresses ?? new List<AddressEntity>();

                AddressEntity addAe = new AddressEntity {Province = "تهران", City = "شهر تهران"};
                mainEntity.Addresses.Add(addAe);

                AddressEntity deleteAe = mainEntity.Addresses.FirstOrDefault(addressEntity => addressEntity.Id == 3);
                mainEntity.Addresses.Remove(deleteAe);
                if (deleteAe.Persons.Count == 1)
                    repository.Remove<AddressEntity>(deleteAe);

                var editAe = mainEntity.Addresses?.FirstOrDefault(addressEntity => addressEntity.Id == 1);
                editAe.Province = "فلسطین";
                editAe.City = "شهر غزه";

                // انجام ویرایش و ذخیره
                repository.Edit(mainEntity.Id, mainEntity);
                repository.SaveChanges();

                // بازیابی شناسه های مربوط به موجودیت های اضافه شده
                int addAeId = addAe.Id;
                int addMneId = addMne.Id;
            }

            ///////////////

            //using (IRepositoryBase<PersonEntity> repository = new Repository<PersonEntity>(new PhoneBookDbContext()))
            //{
            //    await repository.InitializeDatabaseAsync();

            //    var entity = repository.FindByLazyLoadingMode<PersonEntity>(1);
            //    var mne = repository.FindByLazyLoadingMode<MobileNumberEntity>(2);
            //    entity.MobileNumbers.Remove(mne);

            //    repository.Edit(entity.Id, entity);
            //    repository.SaveChanges();
            //}


            //TestInDataAccess testInDataAccess = new TestInDataAccess();
            //await testInDataAccess.Test();
        }
    }
}
