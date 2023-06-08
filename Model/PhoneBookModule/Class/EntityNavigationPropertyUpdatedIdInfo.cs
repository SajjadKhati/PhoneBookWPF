using DataAccess.EntityModule.Class.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.PhoneBookModule.Class
{
    internal class EntityNavigationPropertyUpdatedIdInfo
    {
        internal IDictionary<MobileNumberEntity, Mobile> PersonMobileUpdatedIds { get; set; }


        internal IDictionary<AddressEntity, Address> PersonAddressUpdatedIds { get; set; }
    }
}
