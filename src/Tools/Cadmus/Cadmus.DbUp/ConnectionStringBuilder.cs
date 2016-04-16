using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cadmus.DbUp.Interfaces;

namespace Cadmus.DbUp
{
    public class ConnectionStringBuilder : IConnectionStringBuilder
    {
        public string Build()
        {
            var connName = IoC.GetConnectionName();
            if (ConfigurationManager.ConnectionStrings[connName] == null)
                return null;
            return ConfigurationManager.ConnectionStrings[connName].ConnectionString;
        }
    }
}
