using DataAccess.ExceptionModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ValidationModule.Class
{
    public class CityValidator : FirstNameValidator
    {
        private string _validationValue;




        public CityValidator(string validationValue) : base(validationValue)
        {
            validationValue = validationValue ?? throw new ArgumentNullException(nameof(validationValue),
                ExceptionMessage.argumentNullExceptionMessage);

            this._validationValue = validationValue;
        }



        protected override void SetErrorMessage(ValidType validType)
        {
            switch (validType)
            {
                case ValidType.LengthValidation:
                    this.ErrorMessage = "حداکثر تعداد کاراکتر برای شهر ، نباید بیشتر از 50 کاراکتر باشد .";
                    break;
                case ValidType.Validation:
                    this.ErrorMessage = "اطلاعات وارد شده برای شهر ، نامعتبر هست .";
                    break;
            }
        }


    }
}
