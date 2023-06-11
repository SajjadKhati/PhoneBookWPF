using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DataAccess.ExceptionModule;
using Model.ValidationModule.Interface;

namespace Model.ValidationModule.Class
{
    public class PostalCodeValidator : MobileNumberValidator
    {
        private string _validationValue;




        public PostalCodeValidator(long validationValue) : base(validationValue.ToString())
        {
            this._validationValue = validationValue.ToString();
        }




        protected override bool HasExactLength()
        {
            return this._validationValue.Length == 10;
        }


        protected override void SetErrorMessage(ValidType validType)
        {
            switch (validType)
            {
                case ValidType.DigitValidation:
                    this.ErrorMessage = "مقدار وارد شده برای شماره کد پستی ، فقط باید عدد باشد و نباید کاراکتر دیگری جز عدد باشد .";
                    break;
                case ValidType.ConvertValidation:
                    this.ErrorMessage = "تبدیل شماره کد پستی ، شکست خورد .";
                    break;
                case ValidType.NegativeValidation:
                    this.ErrorMessage = "عدد برای شماره کد پستی ، باید بزرگتر از صفر باشد .";
                    break;
                case ValidType.ExactLengthValidation:
                    this.ErrorMessage = "تعداد اعداد وارد شده برای شماره کد پستی ، باید 10 رقم باشد .";
                    break;
            }
        }


    }
}
