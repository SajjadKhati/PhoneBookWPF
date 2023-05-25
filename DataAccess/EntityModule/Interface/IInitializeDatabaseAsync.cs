using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityModule.Interface
{
    public interface IInitializeDatabaseAsync
    {
        Task InitializeDatabaseAsync(bool force = false);
    }
}
