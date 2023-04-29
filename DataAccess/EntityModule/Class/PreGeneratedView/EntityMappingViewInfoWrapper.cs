using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityModule.Class.PreGeneratedView
{
    [DataContractAttribute()]
    internal class EntityMappingViewInfoWrapper
    {
        [DataMemberAttribute()]
        internal string EntityName { get; set; }


        [DataMemberAttribute()]
        internal string EntityContainerName{ get; set; }


        [DataMemberAttribute()]
        internal string DatabaseSchema { get; set; }


        [DataMemberAttribute()]
        internal string EntitySql{ get; set; }
    }
}
