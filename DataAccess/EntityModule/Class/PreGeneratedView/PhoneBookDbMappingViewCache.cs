using DataAccess.ExceptionModule;
using DataAccess.SerializationModule;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure.MappingViews;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityModule.Class.PreGeneratedView
{
    /// <summary>
    /// در این کلاس  ، نمیدانم به چه علت ، فقط یکبار آن هم زمان ساخت فایل xml برای mapping view ،
    /// اعضای این کلاس فراخوانی میشوند و در دفعات اجرای بعدیِ برنامه ، اعضایش فراخوانی نمیشوند ؛ با آنکه اتریبیوتِ زیر را در فایل assemblyinfo.cs در این پروژه استفاده کردیم :
    /// [assembly: DbMappingViewCacheType(typeof(PhoneBookDbContext), typeof(PhoneBookDbMappingViewCache))]
    /// بنابراین خواندن و استفاده از اطلاعات  Mapping View فعلا در این برنامه استفاده نمیشوند تا به این علت ، سرعت اجرای دفعه ی اول برنامه در دستور EF ، بیشتر شود .
    /// </summary>
    internal class PhoneBookDbMappingViewCache : DbMappingViewCache
    {
        private DbMappingViewInfo dbMappingViewInfo;


        public PhoneBookDbMappingViewCache()
        {
            this.dbMappingViewInfo = this.GetDbMappingViewInfo();
        }


        private DbMappingViewInfo GetDbMappingViewInfo()
        {
            string xmlFullFileName = "PreGeneratedMappingViews.xml";
            string fullFilePath = AppDomain.CurrentDomain.BaseDirectory + xmlFullFileName;
            if (File.Exists(fullFilePath) == false)
                return null;

            DbMappingViewInfo dbMappingViewInfo = XmlDataSerializer.ReadDataFromXmlFile<DbMappingViewInfo>(xmlFullFileName);
            return dbMappingViewInfo;
        }


        public override string MappingHashValue
        {
            get
            {
                return this.dbMappingViewInfo?.DbMappingHashValue;
            }
        }


        public override DbMappingView GetView(EntitySetBase extent)
        {
            if (extent == null) 
                throw new ArgumentNullException(nameof(extent), ExceptionMessage.argumentNullExceptionMessage);

            EntityMappingViewInfoWrapper matchedEntityMappingViewInfoWrapper = this.dbMappingViewInfo?.EntityMappingViews?.Find(
                (EntityMappingViewInfoWrapper currentEntityMappingView) =>
                {
                    return currentEntityMappingView.EntityName == extent.Name;
                });

            if (matchedEntityMappingViewInfoWrapper == null || matchedEntityMappingViewInfoWrapper.EntitySql == null)
                return null;

            DbMappingView dbMappingView = new DbMappingView(matchedEntityMappingViewInfoWrapper.EntitySql);
            return dbMappingView;
        }


    }
}
