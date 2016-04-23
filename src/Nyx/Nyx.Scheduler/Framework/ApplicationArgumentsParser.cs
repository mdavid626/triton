using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fclp;
using Nyx.Scheduler.Interfaces;

namespace Nyx.Scheduler.Framework
{
    public class ApplicationArgumentsParser
    {
        private readonly FluentCommandLineParser<ApplicationArguments> _parser;

        public ApplicationArgumentsParser()
        {
            _parser = new FluentCommandLineParser<ApplicationArguments>();
            var version = new Cadmus.Foundation.AppVersionInfo();
            Version = version.GetCurrentVersion();
            Setup();
        }

        public string Version { get; }

        public IApplicationArguments Arguments => _parser.Object;

        private void Setup()
        {
            _parser.Setup(arg => arg.Help)
                   .As('h', "help")
                   .SetDefault(false);

            _parser.Setup(arg => arg.Version)
                   .As('v', "version")
                   .SetDefault(false);

            _parser.Setup(arg => arg.Schedule)
                   .As('e', "schedule")
                   .SetDefault(false);

            _parser.Setup(arg => arg.Shutdown)
                   .As('s', "shutdown")
                   .SetDefault(false);

            _parser.Setup(arg => arg.Force)
                   .As('f', "force")
                   .SetDefault(false);

            _parser.Setup(arg => arg.UnLock)
                   .As('u', "unlock")
                   .SetDefault(false);
        }

        public ICommandLineParserResult Parse(string[] args)
        {
            var result = _parser.Parse(args);
            return result;
        }

        public void ShowVersion()
        {
            Console.WriteLine(Version);
        }

        public void ShowHelp()
        {
            var version = new Cadmus.Foundation.AppVersionInfo();
            Console.WriteLine("Scheduler - Task Scheduling Tool " + version.GetCurrentVersion() + "\n" +
                              "2016 (C) Cymric\n" +
                              "Usage:\n" +
                              "--help: show help\n" +
                              "--force: run all task even if not scheduled\n" +
                              "--schedule: run tasks\n" +
                              "--shutdown: gracefully end all tasks and shut down scheduler\n" +
                              "--unlock: run tasks even if is locked\n" +
                              "--version: show version\n");
        }
    }
}
