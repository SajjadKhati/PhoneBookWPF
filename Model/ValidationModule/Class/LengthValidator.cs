using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.ExceptionModule;
using Model.ValidationModule.Interface;

namespace Model.ValidationModule.Class
{
    public abstract class LengthValidator : IValidator
    {
        public string ErrorMessage { get; protected set; }




        public abstract bool IsValid();


        protected bool IsLengthValid(object validationValue, int maxLength, int minLength = -1)
        {
            validationValue = validationValue ?? throw new ArgumentNullException(nameof(validationValue),
                ExceptionMessage.argumentNullExceptionMessage);

            string inputValueString = (validationValue is string) ? (string)validationValue : validationValue.ToString();
            if (minLength > -1)
                return inputValueString.Length >= minLength && inputValueString.Length <= maxLength;
            else
                return inputValueString.Length <= maxLength;
        }


    }
}
