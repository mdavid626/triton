using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cadmus.Foundation;

namespace Cadmus.Runner
{
    class Program
    {
        static int Main(string[] args)
        {
            var loggers = new MultiLogger();
            loggers.Add(new FileLogger("deploy.log"));
            loggers.Add(new ConsoleLogger() { IsVerbose = false });
            var runner = new Runner(loggers)
            {
                ExecutablePath = args.FirstOrDefault(),
                Arguments = String.Join(" ", args.Skip(1)),
                WorkingFolder = AppDomain.CurrentDomain.BaseDirectory
            };
            runner.Start();
            if (!runner.Process.HasExited)
                runner.Process.WaitForExit();
            return runner.Process.ExitCode;
        }
    }
}
