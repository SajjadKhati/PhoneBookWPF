using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityModule.Interface
{
    public interface IRepositoryBase<TNonQueryEntity> : IRepository<TNonQueryEntity>, IQueryEagerLoading, IRepositoryAsyncBase, 
    IDisposable where TNonQueryEntity : class
    {

    }
}
