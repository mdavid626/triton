using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cadmus.DbUp.Interfaces;
using Fclp;

namespace Cadmus.DbUp
{
    public enum ReturnCode
    {
        Success = 0,
        ArgumentsError = -1,
        Failure = -2,
        Exception = -3,
    }

    public class Program
    {
        static int Main(string[] args)
        {
            try
            {
                var parser = new ApplicationArgumentsParser();
                var result = parser.Parse(args);

                if (result.HasErrors || result.AdditionalOptionsFound.Any())
                {
                    parser.ShowHelp();
                    return (int)ReturnCode.ArgumentsError;
                }

                RegisterOperations(parser);
                var operation = parser.CreateOperation();
                operation.Execute();
                return (int)ReturnCode.Success;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
                return (int) ReturnCode.Exception;
            }
        }

        static void RegisterOperations(IApplicationArgumentsParser parser)
        {
            var connStringBuilder = new ConnectionStringBuilder();
            var connString = connStringBuilder.Build();

            parser.RegisterOperation(nameof(parser.Arguments.Create), 
                new OperationWrapper(new DatabaseCreator(connString), new InfoLogger()) { Silent = parser.Arguments.Silent });

            parser.RegisterOperation(nameof(parser.Arguments.Drop),
                new OperationWrapper(new DatabaseDropper(connString), new InfoLogger()) { Silent = parser.Arguments.Silent });

            parser.RegisterOperation(nameof(parser.Arguments.Upgrade),
                new OperationWrapper(new DatabaseUpgrader(connString, new DatabasePermissionChecker(connString))
                {
                    Timeout = TimeSpan.FromSeconds(parser.Arguments.Timeout),
                    TransactionOption = parser.Arguments.TransactionOption
                }, new InfoLogger()) { Silent = parser.Arguments.Silent });

            parser.RegisterOperation(nameof(parser.Arguments.Version),
                new OperationWrapper(new InfoLogger(onlyVersion:true), new InfoLogger()) { ShowInfo = false, Silent = true });

            parser.RegisterOperation(nameof(parser.Arguments.Help),
                new OperationWrapper(new ApplicationArgumentsParser(), new InfoLogger()) { ShowInfo  = false, Silent = parser.Arguments.Silent });
        }
    }
}
