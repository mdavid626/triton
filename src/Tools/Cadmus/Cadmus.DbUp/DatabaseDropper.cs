using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cadmus.DbUp.Interfaces;

namespace Cadmus.DbUp
{
    public class DatabaseDropper : IOperation
    {
        private readonly IConnectionStringBuilder _connBuilder;

        public DatabaseDropper(IConnectionStringBuilder connBuilder)
        {
            _connBuilder = connBuilder;
        }

        public void Execute()
        {
            var sqlBuilder = new SqlConnectionStringBuilder(_connBuilder.ConnectionString);
            var dbName = sqlBuilder.InitialCatalog;
            sqlBuilder.InitialCatalog = "master";
            var query = "use master; alter database [" + dbName + "] set single_user with rollback immediate; drop database [" + dbName + "]";

            using (var con = new SqlConnection() { ConnectionString = sqlBuilder.ConnectionString })
            {
                con.Open();
                var cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
                Console.WriteLine("Dropped.");
            }
        }

        public string Name => "DropDatabase";

        public void ShowInfo()
        {
            _connBuilder.ShowInfo();
        }
    }
}
