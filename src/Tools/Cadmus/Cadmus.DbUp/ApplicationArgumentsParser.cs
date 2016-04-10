using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fclp;

namespace Cadmus.DbUp
{
    public class ApplicationArgumentsParser
    {
        private readonly FluentCommandLineParser<ApplicationArguments> _parser;

        public ApplicationArguments Arguments => _parser.Object;

        public ApplicationArgumentsParser()
        {
            _parser = new FluentCommandLineParser<ApplicationArguments>();
            Setup();
        }

        private void Setup()
        {
            _parser.Setup(arg => arg.Help)
                   .As('h', "help")
                   .SetDefault(false);

            _parser.Setup(arg => arg.Silent)
                   .As('s', "silent")
                   .SetDefault(false);

            _parser.Setup(arg => arg.Create)
                   .As('c', "create")
                   .SetDefault(false);

            _parser.Setup(arg => arg.Drop)
                   .As('d', "drop")
                   .SetDefault(false);

            _parser.Setup(arg => arg.Upgrade)
                   .As('u', "upgrade")
                   .SetDefault(false);

            _parser.Setup(arg => arg.Version)
                   .As('v', "version")
                   .SetDefault(false);
        }
        public ICommandLineParserResult Parse(string[] args)
        {
            var result = _parser.Parse(args);

            return result;
        }

        public static void ShowHelp()
        {
            Console.WriteLine("DbUp - Database Upgrade Tool\n" +
                              "2016 (C) Cymric\n" +
                              "Usage:\n" +
                              "--help: show help\n" +
                              "--silent: silent mode, disable confirmations\n" +
                              "--create: create new database from embedded dacpac\n" +
                              "--drop: drop database\n" +
                              "--upgrade: upgrade existing database\n" +
                              "--version: show version\n");
        }
    }
}
