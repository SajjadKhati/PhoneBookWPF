using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityModule.Interface
{
    public interface IQueryEagerLoading
    {
        IEnumerable<TQueryEntity> GetAllByEagerLoadingMode<TQueryEntity>() where TQueryEntity : class;


        TQueryEntity FindByEagerLoadingMode<TQueryEntity>(int id, string idPropertyName = "Id") where TQueryEntity : class;

    }
}
