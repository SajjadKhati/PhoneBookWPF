using DataAccess.EntityModule.Class.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.PhoneBookModule.Class
{
    internal class EntityNavigationPropertyRemovalInfo
    {
        internal IList<MobileNumberEntity> PersonMobileEntitiesToRemove{ get; set; }


        internal IList<AddressEntity> PersonAddressEntitiesToRemove { get; set;}
    }
}
