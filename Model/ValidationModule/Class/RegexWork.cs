using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Model.ValidationModule.Class
{
    internal class RegexWork
    {
        internal static bool IsDigitRegexMatch(string validationText)
        {
            try
            {
                Match noneDigitMatch = Regex.Match(validationText, @"\D");
                if (noneDigitMatch.Success == true)
                    return false;
                else
                    return true;
            }
            catch (ArgumentException argumentExp)
            {
                throw new ArgumentException("یک خطا در ترجمه ی عبارات نامنظم روی داد .", argumentExp);
            }
        }



    }
}
