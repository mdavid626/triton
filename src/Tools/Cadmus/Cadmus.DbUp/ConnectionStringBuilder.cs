using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cadmus.DbUp.Interfaces;

namespace Cadmus.DbUp
{
    public class ConnectionStringBuilder : IConnectionStringBuilder
    {
        public ConnectionStringBuilder()
        {
            ConnectionString = Build();
        }

        public string ConnectionString { get; }

        private string Build()
        {
            var connName = IoC.GetConnectionName();
            if (ConfigurationManager.ConnectionStrings[connName] == null)
                return null;
            return ConfigurationManager.ConnectionStrings[connName].ConnectionString;
        }

        public void ShowInfo()
        {
            var builder = new SqlConnectionStringBuilder(ConnectionString);

            Console.WriteLine("DbServer: " + builder.DataSource);
            Console.WriteLine("DbName: " + builder.InitialCatalog);
            Console.WriteLine("Integrated Security: " + builder.IntegratedSecurity);
            if (!builder.IntegratedSecurity)
                Console.WriteLine("Username: " + builder.UserID);
        }
    }
}
