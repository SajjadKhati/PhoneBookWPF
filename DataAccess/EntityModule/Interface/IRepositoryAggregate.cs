using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityModule.Interface
{
    public interface IRepositoryAggregate<TNonQueryEntity> : IRepository<TNonQueryEntity>, IQueryEagerLoading, 
        IRepositoryAsyncAggregate, IDisposable where TNonQueryEntity : class { }
}
