using DataAccess.ExceptionModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ValidationModule.Class
{
    public class AddressDetailValidator : LengthValidator
    {
        private string _validationValue;




        public AddressDetailValidator(string validationValue)
        {
            validationValue = validationValue ?? throw new ArgumentNullException(nameof(validationValue),
                ExceptionMessage.argumentNullExceptionMessage);

            this._validationValue = validationValue;
        }




        public override bool IsValid()
        {
            bool validationResult;
            validationResult = this.IsLengthValid(this._validationValue, 300);
            if (validationResult == false)
            {
                this.ErrorMessage = "حداکثر تعداد کاراکتر برای جزئیات آدرس ، نباید بیشتر از 300 کاراکتر باشد .";
                throw new Exception(this.ErrorMessage);
            }

            return validationResult;
        }


    }
}
