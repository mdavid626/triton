using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cadmus.DbUp.Interfaces;
using Fclp;

namespace Cadmus.DbUp
{
    public class ApplicationArgumentsParser : IApplicationArgumentsParser, IOperation
    {
        private readonly FluentCommandLineParser<ApplicationArguments> _parser;
        private readonly Dictionary<string, IOperation> _operations;

        public IApplicationArguments Arguments => _parser.Object;

        public ApplicationArgumentsParser()
        {
            _operations = new Dictionary<string, IOperation>();
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

            _parser.Setup(arg => arg.Timeout)
                   .As('t', "timeout")
                   .SetDefault(15);

            _parser.Setup(arg => arg.TransactionOption)
                   .As('o', "transaction")
                   .SetDefault(TransactionOption.TransactionPerScript);
        }
        public ICommandLineParserResult Parse(string[] args)
        {
            var result = _parser.Parse(args);

            return result;
        }

        public void ShowHelp()
        {
            var version = new Cadmus.Foundation.AppVersionInfo();
            Console.WriteLine("DbUp - Database Upgrade Tool " + version.GetCurrentVersion() + "\n" +
                              "2016 (C) Cymric\n" +
                              "Usage:\n" +
                              "--help: show help\n" +
                              "--silent: silent mode, disable confirmations\n" +
                              "--create: create new database from embedded dacpac\n" +
                              "--drop: drop database\n" +
                              "--upgrade: upgrade existing database\n" +
                              "--timeout <value>: script execution timeout in seconds\n" +
                              "--transaction [NoTransaction|Transaction|TransactionPerScript]: transaction options, default is per script\n" +
                              "--version: show version\n");
        }

        public void RegisterOperation(string name, IOperation instance)
        {
            _operations.Add(name, instance);
        }

        private string GetRequestedOperationName()
        {
            if (Arguments.Help)
                return nameof(Arguments.Help);
            if (Arguments.Version)
                return nameof(Arguments.Version);
            if (Arguments.Create)
                return nameof(Arguments.Create);
            if (Arguments.Drop)
                return nameof(Arguments.Drop);
            if (Arguments.Upgrade)
                return nameof(Arguments.Upgrade);
            return nameof(Arguments.Help);
        }

        public IOperation CreateOperation()
        {
            var name = GetRequestedOperationName();
            var operation = _operations.FirstOrDefault(d => d.Key == name);
            return operation.Value;
        }

        public void Execute()
        {
            ShowHelp();
        }

        public string Name => "Help";

        public void ShowInfo()
        {
            
        }
    }
}
