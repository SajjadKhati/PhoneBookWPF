using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure.MappingViews;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityModule.Class.PreGeneratedView
{
    /// <summary>
    /// کلاسی برای ذخیره ی اطلاعات مربوط به MappingViews و MappingHashValue برای serialize و deserialize کردن در فایل
    /// تا در روند PreGeneration View از آن استفاده کنیم .
    /// </summary>
    
    [DataContractAttribute()]
    internal class DbMappingViewInfo
    {
        /// <summary>
        /// شی دیکشنری مربوط به اطلاعات  view ها
        /// </summary>
        [DataMemberAttribute()]
        internal List<EntityMappingViewInfoWrapper> EntityMappingViews{ get; set; }


        /// <summary>
        /// رشته ی حاوی اطلاعات کد Hash برای Mapping مان تا EF با آن بررسی کند که اطلاعات موجودیت های ذخیره شده ،
        /// با اطلاعات موجودیت های زمان اجرای برنامه ، یکی باشد و تغییر نکرده باشد .
        /// </summary>
        [DataMemberAttribute()]
        internal string DbMappingHashValue{ get; set; }
    }
}
