using CommunityToolkit.Mvvm.Input;
using Model.PhoneBookModule.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModel.PhoneBookViewModelModule.Interface
{
    public interface IPhoneBookViewModel
    {
        IPhoneBookAggregator PhoneBook { get; }


        IAsyncRelayCommand LoadPhoneBookAsyncCommand { get; }


        ICommand LoadCitiesByProvinceIdCommand{ get; }

    }
}
