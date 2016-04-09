using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nyx.Web.UiTests.Framework;
using OpenQA.Selenium.Support.UI;

namespace Nyx.Web.UiTests.Pages
{
    public class HomePage
    {
        public static void GoTo()
        {
            Driver.Instance.Navigate().GoToUrl(Driver.BaseAddress);
            Driver.Wait();
        }

        public static bool IsAt
        {
            get { return Driver.Instance.Url == Driver.BaseAddress && Driver.Instance.Title.StartsWith("Home"); }
        }
    }
}
