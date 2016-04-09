using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Nyx.Web.UiTests.Framework
{
    [TestClass]
    public abstract class TestBase
    {
        [TestInitialize]
        public virtual void Init()
        {
            
        }

        [TestCleanup]
        public virtual void Cleanup()
        {
            
        }
        
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            Driver.Initialize();
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            Driver.Close();
        }
    }
}
