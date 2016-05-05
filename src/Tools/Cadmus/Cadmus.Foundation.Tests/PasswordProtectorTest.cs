using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cadmus.Foundation.Tests
{
    [TestClass]
    public class PasswordProtectorTest
    {
        [TestMethod]
        public void TestProtect()
        {
            var password = "password";
            var protector = new PasswordProtector();
            protector.Protect(password, PasswordProtectionScope.LocalMachine);
        }

        [TestMethod]
        public void TestUnProtect()
        {
            var password = "password";
            var protector = new PasswordProtector();
            var protectedPassword = protector.Protect(password, PasswordProtectionScope.LocalMachine);
            var finalPass = protector.UnProtect(protectedPassword);
            Assert.IsTrue(password == finalPass);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestUnProtectInvalidBase64()
        {
            var protector = new PasswordProtector();
            protector.UnProtect("aasdfasdf");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestUnProtectInvalidFormat()
        {
            var protector = new PasswordProtector();
            protector.UnProtect(Convert.ToBase64String(Encoding.UTF8.GetBytes("aaaa")));
        }
    }
}
