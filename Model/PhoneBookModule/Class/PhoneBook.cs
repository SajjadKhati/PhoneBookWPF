using DataAccess.EntityModule.Class;
using DataAccess.EntityModule.Class.Entity;
using DataAccess.ExceptionModule;
using Model.PhoneBookModule.Enum;
using Model.PhoneBookModule.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Model.PhoneBookModule.Class
{
    public class PhoneBook : IPhoneBookAggregator
    {
        /// <summary>
        ///
        /// ارتباط این کلاسِ جاری ، با کلاسی که در این پروپرتی مشخص شد ، ارتباط Aggregation هست . <br/>
        /// چون شی این کلاسِ جاری ، مستقل از شیِ کلاسی که در نوعِ این پروپرتی مشخص شد ، هست .
        /// </summary>
        public IList<Person> People{ get; }


        /// <summary>
        ///
        /// ارتباط این کلاسِ جاری ، با کلاسی که در این پروپرتی مشخص شد ، ارتباط Accociation هست . <br/>
        /// چون شی این کلاسِ جاری ، فقط با شیِ کلاسی که در نوعِ این پروپرتی مشخص شد ، صرفا یک ارتباط دارد .
        /// </summary>
        public IList<Province> Provinces{ get; }




        private bool allowDeleteFlag = true;




        public event EventHandler<DeleteStatusEventArgs> PersonDeleteStatus;


        public event Action AnyPersonOperationCanceled;




        public static object AddPeopleTaskLock = new object();




        public PhoneBook()
        {
            /// اشیاء های زیر را اگر بصورت "تزریق وابستگی" از بیرون دریافت کنیم ، ممکن است کاربر را گمراه کند و آیتم های آنرا هم پُر کند .
            /// در صورتی که آیتم های این اشیاء ، فقط توسط متدهایی که با Load شروع میشوند ، باید پُر شود .
            ObservableCollection<Person> persons = new ObservableCollection<Person>();
            persons.CollectionChanged += People_CollectionChanged;
            this.People = persons;
            this.Provinces = new ObservableCollection<Province>();
        }




        public async Task LoadPeopleAsync()
        {
            IList<Person> allPersons = null;
            using (ContactPhoneBookToDataAccess contactToDataAccess = new ContactPhoneBookToDataAccess())
            {
                await contactToDataAccess.InitializeDatabaseAsync();

                IList<PersonEntity> allPersonEntities = await contactToDataAccess.GetAllAsync<PersonEntity>();
                if (allPersonEntities == null || allPersonEntities.Any() == false)
                    return;

                allPersons = await PersonMapper.MapToPersonListAsync(allPersonEntities);
                if(allPersons == null || allPersons.Any() == false)
                    return;
            }

            await this.AddToPeoplePropertyAsync(allPersons);
        }


        public async Task LoadProvincesAsync()
        {
            IList<Province> provinces = null;
            using (ContactPhoneBookToDataAccess contactToDataAccess = new ContactPhoneBookToDataAccess())
            {
                /// موقع واکشی اطلاعات از پایگاه داده ، در نخ اصلی دریافت میکند . یکی از دلایلش ، زیاد نبود آیتم های استان هاست .
                IList<ProvinceEntity> allProvinceEntities = contactToDataAccess.GetAllByLazyLoadingMode<ProvinceEntity>();
                if (allProvinceEntities == null || allProvinceEntities.Any() == false)
                    return;
                /// اما موقع نگاشت کردن ، در نخ جدیدی انجام میدهد . دلیل خاصی ندارد . هر چند آیتم های کمی دارد .
                provinces = await ProvinceMapper.MapScalarPropertiesToProvinceListAsync(allProvinceEntities);
                if (provinces == null || provinces.Any() == false) 
                    return;
            }
            /// اما موقع اضافه کردن به پروپرتی ، در همین نخِ اصلی انجام میشود .
            this.AddToProvincesProperty(provinces);
        }


        public void LoadCitiesByProvinceId(int provinceId)
        {
            using (ContactPhoneBookToDataAccess contactToDatabase = new ContactPhoneBookToDataAccess())
            {
                ProvinceEntity findedProvinceEntity = contactToDatabase.FindByLazyLoadingMode<ProvinceEntity>(provinceId);
                if (findedProvinceEntity == null)
                    return;
                int indexOfProvince = this.GetIndexOfProvince(provinceId);
                if (indexOfProvince < 0 || this.Provinces[indexOfProvince].Cities.Count > 0)
                    return;

                ProvinceMapper.AddCitiesToProvince(findedProvinceEntity.CityEntities, this.Provinces[indexOfProvince].Cities);
            }
        }


        public bool AddPerson(Person person)
        {
            using (ContactPhoneBookToDataAccess contactToDataAccess = new ContactPhoneBookToDataAccess())
            {
                PersonEntity addPersonEntity = PersonMapper.MapToEntity(person, contactToDataAccess,
                    out EntityNavigationPropertyUpdatedIdInfo personNavigationPropertyUpdatedId);

                contactToDataAccess.AddPersonEntity(addPersonEntity);
                bool isSaved = contactToDataAccess.SaveChanges();

                this.UpdatePersonMemberIdAfterSaved(person, addPersonEntity, personNavigationPropertyUpdatedId);
                return isSaved;
            }
        }


        public bool EditPerson(Person person)
        {
            using (ContactPhoneBookToDataAccess contactToDataAccess = new ContactPhoneBookToDataAccess())
            {
                PersonEntity editPersonEntity = contactToDataAccess.FindByLazyLoadingMode<PersonEntity>(person.Id);
                if (editPersonEntity == null)
                    throw new Exception(ExceptionMessage.entityNotFoundExceptionMessage);

                PersonMapper.UpdateEntityFromPerson(person, editPersonEntity, contactToDataAccess,
                    out EntityNavigationPropertyUpdatedIdInfo personNavigationPropertyUpdatedId);

                contactToDataAccess.EditPersonEntity(editPersonEntity);
                bool isSaved = contactToDataAccess.SaveChanges();

                this.UpdateCollectionsIdAfterSaved(personNavigationPropertyUpdatedId);
                return isSaved;
            }
        }


        private bool DeletePerson(Person person)
        {
            bool isDeletionSuccessed = false;
            Exception deletionException = null;

            try
            {
                using (ContactPhoneBookToDataAccess contactToDataAccess = new ContactPhoneBookToDataAccess())
                {
                    PersonEntity removePersonEntity =
                        contactToDataAccess.FindByLazyLoadingMode<PersonEntity>(person.Id);
                    if (removePersonEntity == null)
                        throw new Exception(ExceptionMessage.entityNotFoundExceptionMessage);

                    /// قبل از حذف ، برای حذف کردن رکورد در موجودیت آدرس ، این متد را فراخوانی کردیم .
                    PersonMapper.RemoveEntityNavigationProperties(removePersonEntity, contactToDataAccess);
                    contactToDataAccess.Remove(removePersonEntity);
                    isDeletionSuccessed = contactToDataAccess.SaveChanges();
                }
            }
            catch (Exception exception)
            {
                deletionException = exception;
                this.AddPersonAfterDeleteFailureByDelay(person);
            }
            finally
            {
                this.PersonDeleteStatus?.Invoke(this, new DeleteStatusEventArgs(isDeletionSuccessed, person, deletionException));
            }

            return isDeletionSuccessed;
        }

        
        private void Person_OperationCanceled(object sender, CancelOperationType e)
        {
            if (e == CancelOperationType.AddCanceled)
            {
                this.allowDeleteFlag = false;
                Person addCanceledPerson = sender as Person;
                if (addCanceledPerson != null)
                {
                    this.People.Remove(addCanceledPerson);
                }
                this.allowDeleteFlag = true;
            }

            this.AnyPersonOperationCanceled?.Invoke();
        }


        private void People_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                IList addedPeople = e.NewItems;
                if (addedPeople == null || addedPeople.Count < 1)
                    return;

                foreach (Person addedPerson in addedPeople)
                {
                    addedPerson.OperationCanceled += this.Person_OperationCanceled;
                }
            }


            if(e.Action == NotifyCollectionChangedAction.Remove && this.allowDeleteFlag == true)
            {
                IList deletionPeople = e.OldItems;
                if (deletionPeople == null || deletionPeople.Count < 1)
                    return;

                foreach (Person deletionPerson in deletionPeople)
                {
                    deletionPerson.OperationCanceled -= this.Person_OperationCanceled;
                    this.DeletePerson(deletionPerson);
                }
            }
        }


        ////////////////////////////////////////////////////////////////////


        /// متدهای خصوصی مربوط به Query


        private async Task AddToPeoplePropertyAsync(IList<Person> people)
        {
            people = people ?? throw new ArgumentNullException(nameof(people), ExceptionMessage.argumentNullExceptionMessage);

            await Task.Factory.StartNew(() =>
            {
                lock (AddPeopleTaskLock)
                {
                    this.AddToPeopleProperty(people);
                }
            });
        }


        private void  AddToPeopleProperty(IList<Person> people)
        {
            people = people ?? throw new ArgumentNullException(nameof(people), ExceptionMessage.argumentNullExceptionMessage);

            this.People.Clear();
            foreach (Person person in people)
            {
                this.People.Add(person);
            }
        }


        private void AddToProvincesProperty(IList<Province> provinces)
        {
            provinces = provinces ?? throw new ArgumentNullException(nameof(provinces), ExceptionMessage.argumentNullExceptionMessage);

            foreach (Province province in provinces)
            {
                this.Provinces.Add(province);
            }
        }


        private int GetIndexOfProvince(int itemIndex)
        {
            Province findedProvince = this.Provinces.FirstOrDefault((Province currentItemProvince) =>
            {
                return currentItemProvince.Id == itemIndex;
            });
            if (findedProvince == null)
                return -1;

            return this.Provinces.IndexOf(findedProvince);
        }


        /// متدهای خصوصی مربوط به Non Query


        private void UpdatePersonMemberIdAfterSaved(Person person, PersonEntity addedPersonEntity, 
            EntityNavigationPropertyUpdatedIdInfo personNavigationPropertyUpdatedId)
        {
            person.Id = addedPersonEntity.Id;
            this.UpdateCollectionsIdAfterSaved(personNavigationPropertyUpdatedId);
        }


        private void UpdateCollectionsIdAfterSaved(EntityNavigationPropertyUpdatedIdInfo entityUpdatedIdInfo)
        {
            if (entityUpdatedIdInfo == null)
                return;

            if (entityUpdatedIdInfo.PersonMobileUpdatedIds != null && entityUpdatedIdInfo.PersonMobileUpdatedIds.Count > 0)
                this.UpdateMobilesIdAfterSaved(entityUpdatedIdInfo.PersonMobileUpdatedIds);

            if (entityUpdatedIdInfo.PersonAddressUpdatedIds != null && entityUpdatedIdInfo.PersonAddressUpdatedIds.Count > 0)
                this.UpdateAddressesIdAfterSaved(entityUpdatedIdInfo.PersonAddressUpdatedIds);
        }


        private void UpdateMobilesIdAfterSaved(IDictionary<MobileNumberEntity, Mobile> mobileUpdateIds)
        {
            foreach (MobileNumberEntity updateIdMobileEntity in mobileUpdateIds.Keys)
            {
                Mobile updateIdMobile = mobileUpdateIds[updateIdMobileEntity];
                updateIdMobile.Id = updateIdMobileEntity.Id;
            }
        }


        private void UpdateAddressesIdAfterSaved(IDictionary<AddressEntity, Address> addressUpdateIds)
        {
            foreach (AddressEntity updateIdAddressEntity in addressUpdateIds.Keys)
            {
                Address updateIdAddress = addressUpdateIds[updateIdAddressEntity];
                updateIdAddress.Id = updateIdAddressEntity.Id;
            }
        }


        private void AddPersonAfterDeleteFailureByDelay(Person person, int delayTimeInMilliSecond = 100)
        {
            /// این شی برای این هست که در نخ جدیدی که در Task کدمان اجرا میشود ، بتوانیم کد را در آن نخ جدید ، درون نخ اصلی اجرا کنیم .
            /// برای همین ، باید این شی را در همان نخی که میخواهیم اجرا کنیم ، ایجاد کنیم . که در اینجا ، همان نخ اصلی هست .
            SynchronizationContext mainThreadSynchronizationContext = SynchronizationContext.Current;

            Task.Delay(delayTimeInMilliSecond).ContinueWith((Task task) =>
            {
                mainThreadSynchronizationContext.Send((object obj) =>
                {
                    /// این متد را در نخ اصلی فراخوانی میکند .
                    this.AddPersonAfterDeleteFailure(person);
                }, null);
            });
        }


        private void AddPersonAfterDeleteFailure(Person person)
        {
            this.People.Add(person);
        }


    }
}
