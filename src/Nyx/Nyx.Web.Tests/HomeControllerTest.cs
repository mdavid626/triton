using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nyx.Web.Controllers;

namespace Nyx.Web.Tests
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void IndexTest()
        {
            var ctrl = new HomeController();
            var result = ctrl.Index() as ViewResult;
            Assert.AreEqual("", result.ViewName);
        }
    }
}
