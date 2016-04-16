using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Cadmus.DbUp.Interfaces;

namespace Cadmus.DbUp
{
    public class InfoLogger : IOperation, IInfoLogger
    {
        private readonly bool _onlyVersion;

        public InfoLogger() : this(false)
        {
            
        }

        public InfoLogger(bool onlyVersion)
        {
            _onlyVersion = onlyVersion;
            var versionInfo = new Cadmus.Foundation.AppVersionInfo();
            Version = versionInfo.GetCurrentVersion();
        }

        public string Version { get; }

        public void ShowInfo()
        {
            Console.WriteLine("DbUp - Database Upgrade Tool " + Version);
            Console.WriteLine("2016 (C) Cymric");
            Console.WriteLine("To show help use: dbup.exe --help");
            Console.WriteLine("Local user: {0}", WindowsIdentity.GetCurrent()?.Name);
            Console.WriteLine("Date: {0}", DateTime.Now);
        }

        public void ShowVersion()
        {
            Console.WriteLine(Version);
        }

        public void Execute()
        {
            if (_onlyVersion)
                ShowVersion();
            else
                ShowInfo();
        }

        public string Name => _onlyVersion ? "Version" : "Info";

        public bool AreYouSure(string message)
        {
            Console.WriteLine(message);
            Console.Write("[Y] Yes  [N] No  [C] Cancel  (default is [Y]): ");
            var key = Console.ReadKey();
            Console.WriteLine();
            return key.Key == ConsoleKey.Y || key.Key == ConsoleKey.Enter;
        }
    }
}
