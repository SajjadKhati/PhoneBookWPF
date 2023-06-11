using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.ValidationModule.Class;

namespace Model.ValidationModule.Class
{
    public class LastNameValidator : FirstNameValidator
    {
        public LastNameValidator(string validationValue) : base(validationValue)
        {
        }


        protected override void SetErrorMessage(ValidType validType)
        {
            switch (validType)
            {
                case ValidType.LengthValidation:
                    this.ErrorMessage = "حداکثر تعداد کاراکتر برای نام خانوادگی شخص ، نباید بیشتر از 50 کاراکتر باشد .";
                    break;
                case ValidType.Validation:
                    this.ErrorMessage = "اطلاعات وارد شده برای نام خانوادگی شخص ، نامعتبر هست .";
                    break;
            }
        }


    }


}
