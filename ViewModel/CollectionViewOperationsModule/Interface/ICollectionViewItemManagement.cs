using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModel.CollectionViewOperationsModule.Interface
{
    public interface ICollectionViewItemManagement
    {
        ICommand AddItemToCollectionViewCommand{ get; }


        ICommand DeleteItemFromCollectionViewCommand { get; }


    }
}
