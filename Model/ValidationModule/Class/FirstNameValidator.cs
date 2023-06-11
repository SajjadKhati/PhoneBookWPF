using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DataAccess.ExceptionModule;
using Model.ValidationModule.Interface;

namespace Model.ValidationModule.Class
{
    public class FirstNameValidator : LengthValidator
    {
        private string _validationValue;




        public FirstNameValidator(string validationValue)
        {
            validationValue = validationValue ?? throw new ArgumentNullException(nameof(validationValue), 
                ExceptionMessage.argumentNullExceptionMessage);

            this._validationValue = validationValue;
        }




        /// <summary>
        /// بخاطر اجرای اصل Liskov در solid ، چون کلاس دیگری از این ارث بری و استفاده میکند ، مستقیما درون این متد ، در صورت نیاز ، پروپرتیِ ErrorMessage مقداردهی نمیشود
        /// و متد دیگری که virtual هست ، فراخوانی میشود تا زمانی که در کلاس فرزند ، این متد را فراخوانی میکنیم ، نیاز به تغییر این متد در کلاس فرزند نداشته باشیم تا اصل liskov نقض شود .
        /// بلکه فقط نیاز به تغییر متد دیگری _SetErrorMessage_ که virtual هست ، داریم .
        /// </summary>
        /// <param name="validationText"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public override bool IsValid()
        {
            bool validationResult;
            validationResult = this.IsLengthValid(this._validationValue, 50);
            if (validationResult == false)
            {
                this.SetErrorMessage(ValidType.LengthValidation);
                throw new Exception(this.ErrorMessage);
            }

            validationResult = this.IsNameValid();
            return validationResult;
        }


        protected virtual void SetErrorMessage(ValidType validType)
        {
            switch (validType)
            {
                case ValidType.LengthValidation:
                    this.ErrorMessage = "حداکثر تعداد کاراکتر برای نام شخص ، نباید بیشتر از 50 کاراکتر باشد .";
                    break;
                    case ValidType.Validation:
                        this.ErrorMessage = "اطلاعات وارد شده برای نام شخص ، نامعتبر هست .";
                        break;
            }
        }


        private bool IsNameValid()
        {
            bool validationResult;
            try
            {
                validationResult = Regex.Match(this._validationValue, @"^[\p{L}\p{M}' \.\-]+$").Success;
                if (validationResult == false)
                {
                    this.SetErrorMessage(ValidType.Validation);
                    throw new Exception(this.ErrorMessage);
                }

                return validationResult;
            }
            catch (ArgumentException argumentExp)
            {
                throw new ArgumentException("یک خطا در ترجمه ی عبارات نامنظم روی داد .", argumentExp);
            }
        }


    }
}
