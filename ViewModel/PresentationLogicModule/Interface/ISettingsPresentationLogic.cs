using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModel.PresentationLogicModule.Interface
{
    public interface ISettingsPresentationLogic
    {
        ICommand ChangeApplicationStyleCommand { get; }

    }
}
