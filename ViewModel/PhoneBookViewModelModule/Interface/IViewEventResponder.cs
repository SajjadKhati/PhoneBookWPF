using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.PhoneBookViewModelModule.Interface
{
    public interface IViewEventResponder
    {
        bool HasValuePostalCodeOrAddressDetails(object person);


        string GetPersonInfo(object person);
    }
}
