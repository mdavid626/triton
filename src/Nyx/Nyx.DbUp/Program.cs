using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nyx.DbUp
{
    class Program
    {
        static int Main(string[] args)
        {
            Cadmus.DbUp.IoC.GetDbRunAssembly = Assembly.GetEntryAssembly;
            return Cadmus.DbUp.Program.Main(args);
        }
    }
}
