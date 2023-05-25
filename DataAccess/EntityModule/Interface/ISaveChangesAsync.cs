using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityModule.Interface
{
    public interface ISaveChangesAsync
    {
        Task<int> SaveChangesAsync();
    }
}
