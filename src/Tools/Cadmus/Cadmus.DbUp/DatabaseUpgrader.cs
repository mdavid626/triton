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
        private readonly IConnectionStringBuilder _connBuilder;
        protected IDatabasePermissionChecker DbChecker;

        public DatabaseUpgrader(IConnectionStringBuilder connBuilder, IDatabasePermissionChecker dbChecker)
        {
            _connBuilder = connBuilder;
            DbChecker = dbChecker;
        }
        public TimeSpan Timeout { get; set; }

        public TransactionOption TransactionOption { get; set; }

        public void Execute()
        {
            DbChecker.Check();

            var upgraderBuilder = DeployChanges.To
                .SqlDatabase(_connBuilder.ConnectionString)
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
        public void ShowInfo()
        {
            _connBuilder.ShowInfo();
            Console.WriteLine("TransactionOption: " + TransactionOption);
            Console.WriteLine("Timeout: " + Timeout);
        }

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
