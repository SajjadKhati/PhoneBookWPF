using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ViewModel.CollectionViewOperationsModule.Enum;

namespace ViewModel.CollectionViewOperationsModule.Interface
{
    public interface ICollectionViewGrouping
    {
        PersonGroupingProperties GroupingProperty{ get; set; }


        ICommand PersonGroupingCommand{ get; }
    }
}
