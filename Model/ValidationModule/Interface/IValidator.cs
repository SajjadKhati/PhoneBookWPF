using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ValidationModule.Interface
{
    /// <summary>
    /// این اینترفیس ، برای ماژول Validate کردن هست .
    /// مطابق با اصل باز و بسته و همچنین اصل وارونگی وابستگی در solid
    /// 
    /// هر چند میشد از اینترفیس آماده ی System.Web.UI.IValidator در دات نت هم استفاده کرد .
    /// </summary>
    public interface IValidator
    {
        string ErrorMessage{ get; }


        bool IsValid();
    }

}
