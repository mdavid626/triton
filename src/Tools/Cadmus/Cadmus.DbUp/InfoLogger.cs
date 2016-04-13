using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Cadmus.DbUp
{
    public class InfoLogger
    {
        public void ShowInfo()
        {
            var version = new Cadmus.Foundation.AppVersionInfo();
            Console.WriteLine("DbUp - Database Upgrade Tool " + version.GetCurrentVersion());
            Console.WriteLine("2016 (C) Cymric");
            Console.WriteLine("To show help use: dbup.exe --help");
            Console.WriteLine("Local user: {0}", WindowsIdentity.GetCurrent().Name);
            Console.WriteLine("Date: {0}", DateTime.Now);
        }
    }
}
