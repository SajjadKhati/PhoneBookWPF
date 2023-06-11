using Model.PhoneBookModule.Class;
using Model.ValidationModule.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.ExceptionModule;

namespace Model.ValidationModule.Class
{
    public class PersonValidator : IValidator
    {
        private Person _person;




        public string ErrorMessage { get; private set; }




        public PersonValidator(Person person)
        {
            person = person ?? throw new ArgumentNullException(nameof(person), ExceptionMessage.argumentNullExceptionMessage);

            this._person = person;
        }




        public bool IsValid()
        {
            bool isValid = true;
            isValid = this.IsFirstNameValid();
            isValid = this.IsLastNameValid();
            isValid = this.IsMobileNumberValid();
            isValid = this.IsAddressValid();

            return isValid;
        }


        private bool IsFirstNameValid()
        {
            bool isValid;
            if (string.IsNullOrEmpty(this._person.FirstName) == true)
            {
                this.ErrorMessage = "مقدار نام شخص ، نمیتواند خالی باشد .";
                throw new Exception(this.ErrorMessage);
            }

            IValidator firstNameValidator = new FirstNameValidator(this._person.FirstName);
            isValid = firstNameValidator.IsValid();
            this.ErrorMessage = firstNameValidator.ErrorMessage;

            return isValid;
        }


        private bool IsLastNameValid()
        {
            bool isValid;
            if (string.IsNullOrEmpty(this._person.LastName) == true)
            {
                this.ErrorMessage = "مقدار نام خانوادگی شخص ، نمیتواند خالی باشد .";
                throw new Exception(this.ErrorMessage);
            }

            IValidator lastNameValidator = new LastNameValidator(this._person.LastName);
            isValid = lastNameValidator.IsValid();
            this.ErrorMessage = lastNameValidator.ErrorMessage;

            return isValid;
        }


        private bool IsMobileNumberValid()
        {
            bool isValid = false;
            if (this._person.Mobiles == null || this._person.Mobiles.Any() == false)
            {
                this.ErrorMessage = "حداقل یک شماره همراه باید ثبت شود.";
                throw new Exception(this.ErrorMessage);
            }

            foreach (Mobile mobile in this._person.Mobiles)
            {
                IValidator mobileValidator = new MobileNumberValidator(mobile.MobileNumber);
                isValid = mobileValidator.IsValid();
                this.ErrorMessage = mobileValidator.ErrorMessage;
            }
            
            return isValid;
        }


        private bool IsAddressValid()
        {
            bool isValid = true;
            if (this._person.Addresses == null || this._person.Addresses.Any() == false)
                return true;

            foreach (Address address in this._person.Addresses)
            {
                if (string.IsNullOrEmpty(address.Province) == true)
                {
                    this.ErrorMessage = "نام استان نمیتواند خالی باشد .";
                    throw new Exception(this.ErrorMessage);
                }
                IValidator provinceValidator = new ProvinceValidator(address.Province);
                isValid = provinceValidator.IsValid();

                if (string.IsNullOrEmpty(address.City) == true)
                {
                    this.ErrorMessage = "نام شهر نمیتواند خالی باشد .";
                    throw new Exception(this.ErrorMessage);
                }
                IValidator cityValidator = new CityValidator(address.City);
                isValid = cityValidator.IsValid();

                if (string.IsNullOrEmpty(address.AddressDetail) == false)
                {
                    IValidator addressDetailValidator = new AddressDetailValidator(address.AddressDetail);
                    isValid = addressDetailValidator.IsValid();
                }

                if (address.PostalCode.HasValue == true)
                {
                    IValidator postalCodeValidator = new PostalCodeValidator(address.PostalCode.Value);
                    isValid = postalCodeValidator.IsValid();
                }
            }

            return isValid;
        }


    }
}
