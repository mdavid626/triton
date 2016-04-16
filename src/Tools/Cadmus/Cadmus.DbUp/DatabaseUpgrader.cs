using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Cadmus.DbUp.Interfaces;
using DbUp;
using DbUp.Builder;
using DbUp.ScriptProviders;
using DbUp.Support.SqlServer;

namespace Cadmus.DbUp
{
    public class DatabaseUpgrader : IOperation
    {
        protected IDatabasePermissionChecker DbChecker;

        public DatabaseUpgrader(string connectionString, IDatabasePermissionChecker dbChecker)
        {
            ConnectionString = connectionString;
            DbChecker = dbChecker;
        }
        public string ConnectionString { get; }

        public TimeSpan Timeout { get; set; }

        public TransactionOption TransactionOption { get; set; }

        public void Execute()
        {
            DbChecker.Check();

            Console.WriteLine("TransactionOption: " + TransactionOption);
            Console.WriteLine("Timeout: " + Timeout);

            var upgraderBuilder = DeployChanges.To
                .SqlDatabase(ConnectionString)
                .WithExecutionTimeout(Timeout)
                .WithScriptsAndCodeEmbeddedInAssembly(IoC.GetDbRunAssembly())
                .LogToConsole();

            upgraderBuilder = ConfigureTransactions(upgraderBuilder);

            var upgrader = upgraderBuilder.Build();
            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
                throw result.Error;
        }

        public string Name => "UpgradeDatabase";

        private UpgradeEngineBuilder ConfigureTransactions(UpgradeEngineBuilder builder)
        {
            if (TransactionOption == TransactionOption.Transaction)
                return builder.WithTransaction();
            if (TransactionOption == TransactionOption.TransactionPerScript)
                return builder.WithTransactionPerScript();
            return builder.WithoutTransaction();
        }
    }
}
