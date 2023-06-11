using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ValidationModule.Class
{
    public enum ValidType
    {
        LengthValidation = 2,
        Validation = 4,
        DigitValidation = 8,
        ConvertValidation = 16,
        NegativeValidation = 32,
        ExactLengthValidation = 64
    }
}
