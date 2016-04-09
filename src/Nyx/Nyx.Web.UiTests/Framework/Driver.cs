using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;

namespace Nyx.Web.UiTests.Framework
{
    public static class Driver
    {
        public static RemoteWebDriver Instance { get; set; }

        public static string BaseAddress
        {
            get
            {
#if DEBUG
                return "http://localhost:9752/";
#else
                return "http://localhost:9752/";
#endif
            }
        }

        public static void Initialize()
        {
            Instance = new InternetExplorerDriver(new InternetExplorerOptions()
            {
                IntroduceInstabilityByIgnoringProtectedModeSettings = true
            });
        }

        public static void Close()
        {
            Instance.Dispose();
        }

        public static void Wait()
        {
            Wait(TimeSpan.FromSeconds(2));
        }

        public static void Wait(TimeSpan timeSpan)
        {
            Thread.Sleep((int)(timeSpan.TotalSeconds * 1000));
        }

        public static string CreateUrl(string url)
        {
            return BaseAddress + url;
        }
    }
}
