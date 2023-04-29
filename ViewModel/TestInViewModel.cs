using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class TestInViewModel
    {
        public async Task Test()
        {
            TestInModel testInModel = new TestInModel();
            await testInModel.Test();
        }
    }
}
