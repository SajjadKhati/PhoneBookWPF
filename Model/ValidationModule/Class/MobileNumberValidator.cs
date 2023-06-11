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
    public class MobileNumberValidator : IValidator
    {
        private string _validationValue;




        public string ErrorMessage { get; protected set; }




        public MobileNumberValidator(string validationValue)
        {
            validationValue = validationValue ?? throw new ArgumentNullException(nameof(validationValue),
                ExceptionMessage.argumentNullExceptionMessage);

            this._validationValue = validationValue;
        }




        public virtual bool IsValid()
        {
            bool isDigit = RegexWork.IsDigitRegexMatch(this._validationValue);
            if (isDigit == false)
            {
                this.SetErrorMessage(ValidType.DigitValidation);
                throw new Exception(this.ErrorMessage);
            }
            else
            {
                bool isConverted = long.TryParse(this._validationValue, out long phoneNumber);
                if (isConverted == false)
                {
                    this.SetErrorMessage(ValidType.ConvertValidation);
                    throw new Exception(this.ErrorMessage);
                }
                else
                {
                    if (phoneNumber <= 0)
                    {
                        this.SetErrorMessage(ValidType.NegativeValidation);
                        throw new Exception(this.ErrorMessage);
                    }
                }

                bool hasExactLength = this.HasExactLength();
                if (hasExactLength == false)
                {
                    this.SetErrorMessage(ValidType.ExactLengthValidation);
                    throw new Exception(this.ErrorMessage);
                }

                return true;
            }
        }


        protected virtual bool HasExactLength()
        {
            return this._validationValue.Length == 11;
        }


        protected virtual void SetErrorMessage(ValidType validType)
        {
            switch (validType)
            {
                case ValidType.DigitValidation:
                    this.ErrorMessage = "مقدار وارد شده برای شماره همراه ، فقط باید عدد باشد و نباید کاراکتر دیگری جز عدد باشد .";
                    break;
                case ValidType.ConvertValidation:
                    this.ErrorMessage = "تبدیل شماره همراه، شکست خورد .";
                    break;
                case ValidType.NegativeValidation:
                    this.ErrorMessage = "عدد برای شماره همراه ، باید بزرگتر از صفر باشد .";
                    break;
                case ValidType.ExactLengthValidation:
                    this.ErrorMessage = "تعداد اعداد وارد شده برای شماره همراه ، باید 11 رقم باشد .";
                    break;
            }
        }


    }
}
