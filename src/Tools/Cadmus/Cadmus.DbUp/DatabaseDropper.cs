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
        public DatabaseDropper(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; }

        public void Execute()
        {
            var sqlBuilder = new SqlConnectionStringBuilder(this.ConnectionString);
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
    }
}
