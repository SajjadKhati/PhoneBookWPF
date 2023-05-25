using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityModule.Interface
{
    public interface IRepository<TNonQueryEntity> where TNonQueryEntity : class
    {
        IEnumerable<TQueryEntity> GetAllByLazyLoadingMode<TQueryEntity>() where TQueryEntity : class;


        TQueryEntity FindByLazyLoadingMode<TQueryEntity>(int id) where TQueryEntity : class;


        TNonQueryEntity Add(TNonQueryEntity entity);


        void Edit(int entityIdInDatabase, TNonQueryEntity entityForEdit);


        TRemoveEntity Remove<TRemoveEntity>(TRemoveEntity removeEntity) where TRemoveEntity : class;


        int SaveChanges();

    }
}
