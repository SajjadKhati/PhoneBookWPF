using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityModule.Interface
{
    public interface IGetAllAsync 
    {
        Task<IEnumerable<TQueryEntity>> GetAllByEagerLoadingModeAsync<TQueryEntity>() where TQueryEntity : class;
    }
}
