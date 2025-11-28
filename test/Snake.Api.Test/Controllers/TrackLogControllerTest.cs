using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snake.Client;

namespace Snake.Api.Test.Controllers
{
    [TestClass]
    public class TrackLogControllerTest
    {
        [TestMethod]
        public void PublishAppLog()
        {
            LogProxy.Error("Exception：测试异常！");
        }
    }
}
