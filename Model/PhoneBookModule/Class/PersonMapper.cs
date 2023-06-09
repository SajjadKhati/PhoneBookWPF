using DataAccess.EntityModule.Class.Entity;
using DataAccess.ExceptionModule;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Model.PhoneBookModule.Class
{
    internal static class PersonMapper
    {

        private static Dictionary<Address, IList<int>> address_PersonIdsDictionary;


        private static Person address_MainPerson;




        #region متدهای مربوط به Query


        internal static async Task<IList<Person>> MapToPersonListAsync(IList<PersonEntity> personEntities)
        {
            return await Task.Factory.StartNew<IList<Person>>((object taskPersonEntities) =>
            {
                IList<PersonEntity> taskPersonEntitiesConvert = (taskPersonEntities as IList<PersonEntity>) ?? 
                                                                throw new ArgumentNullException(nameof(taskPersonEntities), ExceptionMessage.argumentNullExceptionMessage);

                return MapToPersonList(taskPersonEntitiesConvert);
            }, personEntities);
        }


        internal static IList<Person> MapToPersonList(IList<PersonEntity> personEntities)
        {
            IList<Person> people = new List<Person>();

            foreach (PersonEntity personEntity in personEntities)
            {
                Person person = MapToPerson(personEntity);
                if(person != null)
                    people.Add(person);
            }

            if (address_PersonIdsDictionary != null)
            {
                CopyEntityToAddressesPersonCollection(people);
                address_PersonIdsDictionary = null;
            }

            return people;
        }


        internal static Person MapToPerson(PersonEntity personEntity)
        {
            Person personToFill = new Person();
            CopyEntityToPerson(personEntity, personToFill);
            return personToFill;
        }


        #endregion


        #region متدهای مربوط به NonQuery


        internal static PersonEntity MapToEntity(Person person, ContactPhoneBookToDataAccess contactToDataAccess,
            out EntityNavigationPropertyUpdatedIdInfo personNavigationPropertyUpdatedIds)
        {
            PersonEntity personEntityToFill = new PersonEntity();
            personNavigationPropertyUpdatedIds = CopyPersonToEntity(person, personEntityToFill, contactToDataAccess);
            return personEntityToFill;
        }


        internal static void UpdateEntityFromPerson(Person person, PersonEntity personEntity, 
            ContactPhoneBookToDataAccess contactToDataAccess,
            out EntityNavigationPropertyUpdatedIdInfo personNavigationPropertyUpdatedIds)
        {
            RemoveEntityNavigationProperties(personEntity, contactToDataAccess);
            personNavigationPropertyUpdatedIds = CopyPersonToEntity(person, personEntity, contactToDataAccess);
        }


        #endregion


        //////////////////////////////////////////////////////////////


        #region  متدهای خصوصی مربوط به NonQuery


        internal static void RemoveEntityNavigationProperties(PersonEntity personEntity, 
            ContactPhoneBookToDataAccess contactToDataAccess)
        {
            RemoveMobileNumberEntities(personEntity.MobileNumbers, contactToDataAccess);
            RemoveAddressEntities(personEntity.Addresses, contactToDataAccess);
        }


        private static void RemoveMobileNumberEntities(ICollection<MobileNumberEntity> mobileEntities,
            ContactPhoneBookToDataAccess contactToDataAccess)
        {
            if (mobileEntities == null || mobileEntities.Any() == false)
                return;

            foreach (MobileNumberEntity mobileEntity in mobileEntities.ToList())
            {
                mobileEntities.Remove(mobileEntity);

                /// وظیفه ی این متد و کلا این کلاس ، ذخیره ی تغییرات نیست .
                /// پس فقط موجودیت مورد نظر را حذف میکنیم تا نگاشت مد نظرمان به درستی انجام شود اما ذخیره نمیکنیم .
                contactToDataAccess.Remove(mobileEntity);
            }
        }


        private static void RemoveAddressEntities(ICollection<AddressEntity> addressEntities,
            ContactPhoneBookToDataAccess contactToDataAccess)
        {
            if (addressEntities == null || addressEntities.Any() == false)
                return;

            foreach (AddressEntity addressEntity in addressEntities.ToList())
            {
                addressEntities.Remove(addressEntity);

                /// اگر کالکشن Persons در موجودیت addressEntity ، چندین عضو داشت ،
                /// یعنی آن آدرس برای چندین نفر هست ، پس آن آدرس را حذف نکند .
                if (addressEntity.Persons == null || addressEntity.Persons.Count <= 1)
                    /// وظیفه ی این متد و کلا این کلاس ، ذخیره ی تغییرات نیست .
                    /// پس فقط موجودیت مورد نظر را حذف میکنیم تا نگاشت مد نظرمان به درستی انجام شود اما ذخیره نمیکنیم .
                    contactToDataAccess.Remove(addressEntity);
            }
        }


        private static EntityNavigationPropertyUpdatedIdInfo CopyPersonToEntity(Person person, PersonEntity personEntity,
            ContactPhoneBookToDataAccess contactToDataAccess)
        {
            person = person ?? throw new ArgumentNullException(nameof(person), ExceptionMessage.argumentNullExceptionMessage);
            personEntity = personEntity ?? throw new ArgumentNullException(nameof(personEntity),
                ExceptionMessage.argumentNullExceptionMessage);

            address_MainPerson = person;

            personEntity.Id = person.Id;
            personEntity.FirstName = person.FirstName;
            personEntity.LastName = person.LastName;
            EntityNavigationPropertyUpdatedIdInfo personNavigationPropertyUpdatedIds = new EntityNavigationPropertyUpdatedIdInfo
            {
                PersonMobileUpdatedIds = CopyMobileCollectionToEntity(person.Mobiles, personEntity),
                PersonAddressUpdatedIds = CopyAddressCollectionToEntity(person.Addresses, personEntity, contactToDataAccess)
            };

            address_MainPerson = null;
            return personNavigationPropertyUpdatedIds;
        }


        private static IDictionary<MobileNumberEntity, Mobile> CopyMobileCollectionToEntity(IList<Mobile> personMobiles, 
            PersonEntity personEntity)
        {
            if (personMobiles == null || personMobiles.Count < 1)
                return null;

            IDictionary<MobileNumberEntity, Mobile> personMobileUpdatedIds = new Dictionary<MobileNumberEntity, Mobile>(1);
            personEntity.MobileNumbers = personEntity.MobileNumbers ?? new List<MobileNumberEntity>();
            foreach (Mobile mobile in personMobiles)
            {
                MobileNumberEntity mobileEntity = new MobileNumberEntity
                {
                    Id = mobile.Id,
                    MobileNumber = mobile.MobileNumber,
                    PersonEntity = personEntity,
                    PersonEntityId = personEntity.Id
                };
                personMobileUpdatedIds.Add(mobileEntity, mobile);

                personEntity.MobileNumbers.Add(mobileEntity);
            }

            return (personMobileUpdatedIds.Count > 0) ? personMobileUpdatedIds : null;
        }


        private static IDictionary<AddressEntity, Address> CopyAddressCollectionToEntity(IList<Address> personAddresses, 
            PersonEntity personEntity, ContactPhoneBookToDataAccess contactToDataAccess)
        {
            if (personAddresses == null || personAddresses.Count < 1)
                return null;

            IDictionary<AddressEntity, Address> personAddressUpdatedIds = new Dictionary<AddressEntity, Address>();
            personEntity.Addresses = personEntity.Addresses ?? new List<AddressEntity>();
            foreach (Address address in personAddresses)
            {
                AddressEntity addressEntity = new AddressEntity()
                {
                    Id = address.Id,
                    Province = address.Province,
                    City = address.City,
                    Address = address.AddressDetail,
                    PostalCode = address.PostalCode
                };
                CopyAddressesPersonCollectionToEntity(address.Persons, addressEntity, personEntity, contactToDataAccess);
                personAddressUpdatedIds.Add(addressEntity, address);

                personEntity.Addresses.Add(addressEntity);
            }

            return (personAddressUpdatedIds.Count > 0) ? personAddressUpdatedIds : null;
        }


        private static void CopyAddressesPersonCollectionToEntity(IList<Person> addressesPersons, AddressEntity addressEntity,
            PersonEntity defaultPersonEntity, ContactPhoneBookToDataAccess contactToDataAccess)
        {
            /// در هر صورتی ، یعنی چه addressesPersons برابر با null بود یا هیچ آیتمی نداشت یا هر تعداد آیتمی که داشت ،شیِ آدرس ، نهایتا با شیِ defaultPersonEntity
            /// که شیِ اصلی اش هست ، ارتباط دارد ، پس به لیست کالکشن Persons در addressEntity ، اضافه کند .
            addressEntity.Persons = new List<PersonEntity>(1)
            {
                defaultPersonEntity
            };

            if (addressesPersons == null || addressesPersons.Count < 2)
                return;

            /// وگرنه ، با PersonEntity ای که از قبل موجود هست و در دیتابیس هست ، ارتباط دارد
            /// پس باید داخل دیتابیس ، این شی را پیدا کند .
            foreach (Person person in addressesPersons)
            {
                if (person == address_MainPerson)
                    continue;

                PersonEntity findedPersonEntity = contactToDataAccess.FindByLazyLoadingMode<PersonEntity>(person.Id);
                if (findedPersonEntity == null)
                    continue;

                addressEntity.Persons.Add(findedPersonEntity);
            }
        }


        #endregion

        //////////

        #region  متدهای خصوصی مربوط به Query


        //private static EntityNavigationPropertyRemovalInfo ExtractEntityRemovalInfo(PersonEntity beforeUpdatePersonEntity)
        //{
        //    EntityNavigationPropertyRemovalInfo entityRemovalInfo = null;
        //    if (beforeUpdatePersonEntity.MobileNumbers != null && beforeUpdatePersonEntity.MobileNumbers.Count > 0)
        //    {
        //        entityRemovalInfo = new EntityNavigationPropertyRemovalInfo()
        //        {
        //            PersonMobileEntitiesToRemove = new List<MobileNumberEntity>(1)
        //        };

        //        foreach (MobileNumberEntity mobileEntityToRemove in beforeUpdatePersonEntity.MobileNumbers)
        //        {
        //            entityRemovalInfo.PersonMobileEntitiesToRemove.Add(mobileEntityToRemove);
        //        }
        //    }

        //    if (beforeUpdatePersonEntity.Addresses != null && beforeUpdatePersonEntity.Addresses.Count > 0)
        //    {
        //        entityRemovalInfo = entityRemovalInfo ?? new EntityNavigationPropertyRemovalInfo();
        //        entityRemovalInfo.PersonAddressEntitiesToRemove = new List<AddressEntity>(1);

        //        foreach (AddressEntity addressEntityToRemove in beforeUpdatePersonEntity.Addresses)
        //        {
        //            if (addressEntityToRemove.Persons?.Count == 1)
        //                entityRemovalInfo.PersonAddressEntitiesToRemove.Add(addressEntityToRemove);
        //        }
        //    }

        //    return entityRemovalInfo;
        //}


        private static void CopyEntityToPerson(PersonEntity personEntity, Person person)
        {
            personEntity = personEntity ?? throw new ArgumentNullException(nameof(personEntity),
                ExceptionMessage.argumentNullExceptionMessage);
            person = person ?? throw new ArgumentNullException(nameof(person), ExceptionMessage.argumentNullExceptionMessage);

            person.Id = personEntity.Id;
            person.FirstName = personEntity.FirstName;
            person.LastName = personEntity.LastName;
            if (personEntity.MobileNumbers != null && personEntity.MobileNumbers.Count > 0)
                CopyEntityToMobileCollection(personEntity.MobileNumbers, person.Mobiles);

            if (personEntity.Addresses != null && personEntity.Addresses.Count > 0)
                CopyEntityToAddressCollection(personEntity.Addresses, person);
        }


        private static void CopyEntityToMobileCollection(ICollection<MobileNumberEntity> mobileEntities, IList<Mobile> personMobiles)
        {
            mobileEntities = mobileEntities ?? throw new ArgumentNullException(nameof(mobileEntities),
                ExceptionMessage.argumentNullExceptionMessage);
            personMobiles = personMobiles ?? throw new ArgumentNullException(nameof(personMobiles),
                ExceptionMessage.argumentNullExceptionMessage);

            foreach (MobileNumberEntity mobileEntity in mobileEntities)
            {
                Mobile mobile = new Mobile()
                {
                    Id = mobileEntity.Id,
                    MobileNumber = mobileEntity.MobileNumber
                };

                personMobiles.Add(mobile);
            }
        }


        private static void CopyEntityToAddressCollection(ICollection<AddressEntity> addressEntities, Person person)
        {
            addressEntities = addressEntities ?? throw new ArgumentNullException(nameof(addressEntities),
                ExceptionMessage.argumentNullExceptionMessage);
            person = person ?? throw new ArgumentNullException(nameof(person), ExceptionMessage.argumentNullExceptionMessage);

            foreach (AddressEntity addressEntity in addressEntities)
            {
                Address address = new Address()
                {
                    Id = addressEntity.Id,
                    Province = addressEntity.Province,
                    City = addressEntity.City,
                    AddressDetail = addressEntity.Address,
                    PostalCode = addressEntity.PostalCode
                };

                if (addressEntity.Persons != null && addressEntity.Persons.Count == 1)
                    address.Persons.Add(person);
                else if (addressEntity.Persons != null && addressEntity.Persons.Count > 1)
                    FillAddressPersonIdsDictionary(addressEntity, address);

                person.Addresses.Add(address);
            }


        }


        private static void FillAddressPersonIdsDictionary(AddressEntity addressEntity, Address personAddress)
        {
            List<int>  personEntityIds = new List<int>();
            foreach (PersonEntity personEntity in addressEntity.Persons)
            {
                personEntityIds.Add(personEntity.Id);
            }

            if (address_PersonIdsDictionary == null)
                address_PersonIdsDictionary = new Dictionary<Address, IList<int>>();
            address_PersonIdsDictionary.Add(personAddress, personEntityIds);
        }


        private static void CopyEntityToAddressesPersonCollection(IList<Person> searchPeople)
        {
            foreach (Address address in address_PersonIdsDictionary.Keys)
            {
                IList<Person> addressPersonCollection = new List<Person>();

                IList<int> personIds = address_PersonIdsDictionary[address];
                foreach (int personId in personIds)
                {
                    Person addressPerson = FindPerson(personId, searchPeople);
                    if(addressPerson != null)
                        addressPersonCollection.Add(addressPerson);
                }

                if (addressPersonCollection.Count > 0)
                    CopyPersonListItems(addressPersonCollection, address.Persons);
            }
        }


        private static Person FindPerson(int personId, IList<Person> searchPeople)
        {
            return searchPeople.FirstOrDefault((Person currentPersonItem) => currentPersonItem.Id == personId);
        }


        private static void CopyPersonListItems(IList<Person> sourceList, IList<Person> destinationList )
        {
            destinationList.Clear();
            foreach (Person person in sourceList)
            {
                destinationList.Add(person);
            }
        }


        #endregion


    }
}
