using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cadmus.DbUp.Interfaces;
using Microsoft.SqlServer.Dac;

namespace Cadmus.DbUp
{
    public class DatabaseCreator : IOperation
    {
        private readonly IConnectionStringBuilder _connBuilder;

        public DatabaseCreator(IConnectionStringBuilder connBuilder)
        {
            _connBuilder = connBuilder;
        }

        public void Execute()
        {
            var sqlBuilder = new SqlConnectionStringBuilder(_connBuilder.ConnectionString);
            var targetDbName = sqlBuilder.InitialCatalog;
            Create(targetDbName);
        }

        public string Name => "CreateDatabase";

        public void ShowInfo()
        {
            _connBuilder.ShowInfo();
        }

        public void Create(string targetDatabaseName)
        {
            var dacpac = GetDacPackage();

            var service = new DacServices(_connBuilder.ConnectionString);
            var options = new DacDeployOptions
            {
                IncludeTransactionalScripts = true,
                BlockOnPossibleDataLoss = false,
            };

            service.Message += service_Message;
            service.ProgressChanged += service_ProgressChanged;

            service.Deploy(dacpac, targetDatabaseName, true, options);
        }

        private DacPackage GetDacPackage()
        {
            return DacPackage.Load("db.dacpac");
        }

        private void service_ProgressChanged(object sender, DacProgressEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

        private void service_Message(object sender, DacMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
