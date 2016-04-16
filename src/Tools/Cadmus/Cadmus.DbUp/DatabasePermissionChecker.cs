using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cadmus.DbUp.Interfaces;

namespace Cadmus.DbUp
{
    public class DatabasePermissionChecker : IDatabasePermissionChecker
    {
        private readonly IConnectionStringBuilder _connBuilder;

        public readonly List<String> PERMISSIONS = new List<String>
        {
            "CREATE TABLE",
            "CREATE VIEW",
            "CREATE PROCEDURE",
            "CREATE FUNCTION",
            "CREATE DEFAULT",
            "CREATE SCHEMA",
            "SELECT",
            "INSERT",
            "UPDATE",
            "DELETE",
            "REFERENCES",
            "EXECUTE",
            "ALTER"
        };

        public DatabasePermissionChecker(IConnectionStringBuilder connBuilder)
        {
            _connBuilder = connBuilder;
        }

        public void Check()
        {
            var missing = CheckPermissions().ToArray();
            if (missing.Any())
                throw new Exception($"Missing database permissions: {string.Join(", ", missing)}");
        }

        public IEnumerable<String> CheckPermissions()
        {
            var permissions = GetPermissions();
            return PERMISSIONS.Where(per => !permissions.Contains(per));
        }

        private IList<string> GetPermissions()
        {
            const string query = "select permission_name from fn_my_permissions (NULL, 'DATABASE')";
            var permissions = new List<string>();

            using (var con = new SqlConnection() { ConnectionString = _connBuilder.ConnectionString })
            {
                con.Open();

                var command = new SqlCommand(query, con);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var permission = reader[0] as string;
                    if (!String.IsNullOrEmpty(permission))
                        permissions.Add(permission);
                }

                reader.Close();
                con.Close();
            }

            return permissions;
        }
    }
}
