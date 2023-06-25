using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.CollectionViewOperationsModule.Interface
{
    public interface ICollectionViewOperationsAggregator : ICollectionViewItemManagement, ICollectionViewFiltering, 
        ICollectionViewGrouping
    {
    }
}
