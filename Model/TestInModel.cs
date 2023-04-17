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
        public void Test()
        {
            TestInDataAccess testInDataAccess = new TestInDataAccess();
            testInDataAccess.Test();
        }
    }
}
