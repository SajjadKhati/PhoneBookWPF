using DataAccess;
using DataAccess.EntityModule.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class TestInModel
    {
        public async Task Test()
        {
            TestInDataAccess testInDataAccess = new TestInDataAccess();
            await testInDataAccess.Test();
        }
    }
}
