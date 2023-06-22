using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.ExceptionModule;

namespace Model.PhoneBookModule.Class
{
    internal static class PersonCloner
    {
        private static Person _copyPerson;




        internal static Person DeepClonePerson(Person sourcePerson)
        {
            sourcePerson = sourcePerson ?? throw new ArgumentNullException(nameof(sourcePerson), 
                ExceptionMessage.argumentNullExceptionMessage);

            Person personClone = new Person();
            _copyPerson = personClone;
            UpdatePerson(sourcePerson, personClone);

            return personClone;
        }


        internal static void UpdatePerson(Person sourcePerson, Person destinationPerson)
        {
            destinationPerson.Id = sourcePerson.Id;
            /// چون FirstName و LastName از نوع string هستند و نوع رشته هم تغییر ناپذیر هست ، پس نیازی به کپی کردن رشته نیست .
            destinationPerson.FirstName = sourcePerson.FirstName;
            destinationPerson.LastName = sourcePerson.LastName;
            destinationPerson.Mobiles.Clear();
            CopyMobiles(sourcePerson.Mobiles, destinationPerson.Mobiles);
            destinationPerson.Addresses.Clear();
            CopyAddresses(sourcePerson.Addresses, destinationPerson.Addresses);

        }


        private static void CopyMobiles(IList<Mobile> sourceMobiles, IList<Mobile> destinationMobiles)
        {
            foreach (Mobile sourceMobile in sourceMobiles)
            {
                destinationMobiles.Add(sourceMobile.ShallowCopy());
            }
        }


        private static void CopyAddresses(IList<Address> sourceAddresses, IList<Address> destinationAddresses)
        {
            foreach (Address sourceAddress in sourceAddresses)
            {
                destinationAddresses.Add(CopySingleAddress(sourceAddress));
            }
        }


        private static Address CopySingleAddress(Address sourceAddress)
        {
            Address addressClone = new Address();
            addressClone.Id = sourceAddress.Id;
            addressClone.Province = sourceAddress.Province;
            addressClone.City = sourceAddress.City;
            addressClone.AddressDetail = sourceAddress.AddressDetail;
            if (sourceAddress.PostalCode != null && sourceAddress.PostalCode.HasValue)
                addressClone.PostalCode = new long?(sourceAddress.PostalCode.Value);
            if (sourceAddress.Persons != null && sourceAddress.Persons.Count > 0)
                CopyAddressPersons(sourceAddress.Persons, addressClone.Persons);

            return addressClone;
        }


        private static void CopyAddressPersons(IList<Person> sourcePersons, IList<Person> destinationPersons)
        {
            foreach (Person sourcePerson in sourcePersons)
            {
                if (sourcePerson.Id == _copyPerson.Id)
                    destinationPersons.Add(_copyPerson);
                else
                    destinationPersons.Add(sourcePerson);
            }
        }


    }
}
