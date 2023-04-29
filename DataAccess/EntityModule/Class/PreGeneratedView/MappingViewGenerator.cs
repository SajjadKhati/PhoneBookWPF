using DataAccess.ExceptionModule;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.MappingViews;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityModule.Class.PreGeneratedView
{
    internal class DbMappingViewGenerator
    {
        internal static DbMappingViewInfo GetDbMappingViewInfo(DbContext dbContext)
        {
            dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext), ExceptionMessage.argumentNullExceptionMessage);

            StorageMappingItemCollection storageMappingItems = DbMappingViewGenerator.GetStorageMappingItems(dbContext);
            /// متغییر edmSchemaErrors ، برای این ارسال میشود که اگر در بدنه ی متد GenerateViews ، خطاهایی اتفاق افتاد ، آن خطاها را به عنوان آیتم هایی به این متغییر اضافه میکند .
            IList<EdmSchemaError> edmSchemaErrors = new List<EdmSchemaError>();
            Dictionary<EntitySetBase, DbMappingView> mappingViews = storageMappingItems?.GenerateViews(edmSchemaErrors);
            string mappingHashValue = storageMappingItems?.ComputeMappingHashValue();
            
            if (mappingViews == null || mappingHashValue == null)
                return null;

            List<EntityMappingViewInfoWrapper> entityMappingViewInfoWrapper =
                DbMappingViewGenerator.GetEntityMappingViewInfoWrappers(mappingViews);
            if (entityMappingViewInfoWrapper == null)
                return null;

            DbMappingViewInfo dbMappingViewInfo = new DbMappingViewInfo();

            dbMappingViewInfo.EntityMappingViews = entityMappingViewInfoWrapper;
            dbMappingViewInfo.DbMappingHashValue = mappingHashValue;
            return dbMappingViewInfo;
        }


        private static StorageMappingItemCollection GetStorageMappingItems(DbContext dbContext)
        {
            /// چون پروپرتی IObjectContextAdapter.ObjectContext درون کلاس DbContext ، بصورت صریح یا explicite پیاده سازی شد ،
            ///  تبدیل صورت بگیرد .پس برای دسترسی بهش ، حتما باید 
            ObjectContext objectContext = ((IObjectContextAdapter)dbContext).ObjectContext;
            StorageMappingItemCollection mappingCollection =
                (StorageMappingItemCollection) objectContext?.MetadataWorkspace.GetItemCollection(DataSpace.CSSpace);
            return mappingCollection;
        }


        private static List<EntityMappingViewInfoWrapper> GetEntityMappingViewInfoWrappers
        (Dictionary<EntitySetBase, DbMappingView> mappingViewDictionary)
        {
            if (mappingViewDictionary == null)
                return null;

            List<KeyValuePair<EntitySetBase, DbMappingView>> mappingViewKeyValuePairList = mappingViewDictionary.ToList();
            List<EntityMappingViewInfoWrapper> entityMappingViewInfoWrapperList = new List<EntityMappingViewInfoWrapper>(12);
            foreach (KeyValuePair<EntitySetBase, DbMappingView> mappingViewKeyValuePair in mappingViewKeyValuePairList)
            {
                EntityMappingViewInfoWrapper entityMappingViewInfoWrapper = new EntityMappingViewInfoWrapper();
                entityMappingViewInfoWrapper.EntityName = mappingViewKeyValuePair.Key.Name;
                entityMappingViewInfoWrapper.EntityContainerName = mappingViewKeyValuePair.Key.EntityContainer?.Name;
                entityMappingViewInfoWrapper.DatabaseSchema = mappingViewKeyValuePair.Key.Schema;
                entityMappingViewInfoWrapper.EntitySql = mappingViewKeyValuePair.Value.EntitySql;
                entityMappingViewInfoWrapperList.Add(entityMappingViewInfoWrapper);
            }

            return entityMappingViewInfoWrapperList;
        }


    }
}
