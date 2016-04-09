using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nyx.Web.UiTests.Framework;
using Nyx.Web.UiTests.Pages;

namespace Nyx.Web.UiTests.Tests
{
    [TestClass]
    public class HomePageTest : TestBase
    {
        [TestMethod]
        public void ShowHomePageTest()
        {
            HomePage.GoTo();
            Assert.IsTrue(HomePage.IsAt);
        }
    }
}
