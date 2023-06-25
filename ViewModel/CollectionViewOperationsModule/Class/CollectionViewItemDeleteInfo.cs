using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.CollectionViewOperationsModule.Class
{
    public class CollectionViewItemDeleteInfo
    {
        public IEditableCollectionViewAddNewItem EditingCollectionView{ get; set; }


        public int CollectionViewIndex{ get; set; }

    }
}
