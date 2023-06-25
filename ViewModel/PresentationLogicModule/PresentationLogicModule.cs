using Model.PhoneBookModule.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.PresentationLogicModule
{
    public class PresentationLogicModule
    {
        public static bool HasValuePostalCodeOrAddressDetails(object person)
        {
            Person personConverted = person as Person;
            if (personConverted == null)
                return false;

            foreach (Address address in personConverted.Addresses)
            {
                if (HasValuePostalCodeOrAddressDetailsForSingleAddress(address) == true)
                    return true;
            }
            return false;
        }


        private static bool HasValuePostalCodeOrAddressDetailsForSingleAddress(Address address)
        {
            if ((address.PostalCode.HasValue == true && address.PostalCode > 0) || string.IsNullOrEmpty(address.AddressDetail) == false)
                return true;

            return false;
        }


        public static string GetPersonDeleteMessage(object person)
        {
            Person personConverted = person as Person;
            if (personConverted == null)
                return "";

            string message = "آیا مطمئن هستید که قصد دارید تا سطر انتخاب شده با اطلاعات زیر را حذف کنید؟\n\n";
            message = (string.IsNullOrEmpty(personConverted.FirstName) == false) ? "نام  :  " + personConverted.FirstName : "";
            message += (string.IsNullOrEmpty(personConverted.LastName) == false) ? "\nنام خانوادگی  :  " + personConverted.LastName : "";
            message += (personConverted.Mobiles.Count > 0 && personConverted.Mobiles[0].MobileNumber != null) ?
                "\nاولین شماره همراه  :  " + personConverted.Mobiles[0].MobileNumber : "";

            return message;
        }


    }
}
