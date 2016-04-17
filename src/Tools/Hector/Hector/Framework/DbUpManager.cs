using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using EnvDTE;
using Microsoft.VisualStudio.Shell;

namespace Hector.Framework
{
    public class DbUpManager
    {
        public const string DbUpProjectName = "DbUp";

        public DTE DTE => (DTE)Package.GetGlobalService(typeof(DTE));

        public void Upgrade(string dbServer, string dbName)
        {
            Build();
            Parametrize(dbServer, dbName);
            StartDbUp("/upgrade");
        }

        public void Create(string dbServer, string dbName)
        {
            Build();
            Parametrize(dbServer, dbName);
            StartDbUp("/create");
        }

        public void Drop(string dbServer, string dbName)
        {
            Build();
            Parametrize(dbServer, dbName);
            StartDbUp("/drop");
        }

        private Project FindDbUpProject()
        {
            return DTE.Solution.Cast<Project>().FirstOrDefault(p => p.Name.Contains(DbUpProjectName));
        }

        private void Build()
        {
            var dbProj = FindDbUpProject();
            DTE.Solution.SolutionBuild.BuildProject("Debug", dbProj.UniqueName, WaitForBuildToFinish: true);
        }

        private void Parametrize(string dbServer, string dbName)
        {
            if (string.IsNullOrEmpty(dbServer) || string.IsNullOrEmpty(dbName))
                return;

            var outputDir = GetOutputDir();
            var configName = Path.Combine(outputDir, "DbUp.exe.config");
            var xml = XDocument.Load(configName);

            var connStringEl = xml.Descendants("connectionStrings").Descendants().FirstOrDefault();
            if (connStringEl != null)
            {
                var connString = connStringEl.Attribute("connectionString").Value;
                var sqlBuilder = new SqlConnectionStringBuilder(connString);
                sqlBuilder.DataSource = dbServer;
                sqlBuilder.InitialCatalog = dbName;
                connStringEl.SetAttributeValue("connectionString", sqlBuilder.ConnectionString);
            }

            xml.Save(configName);
        }

        private string GetOutputDir()
        {
            var dbProj = FindDbUpProject();
            return Path.Combine(Path.GetDirectoryName(dbProj.FullName), "bin\\Debug");
        }

        private void StartDbUp(string arguments)
        {
            var proc = new System.Diagnostics.Process()
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "DbUp.exe",
                    Arguments = arguments,
                    WorkingDirectory = GetOutputDir(),
                }
            };

            proc.Start();
            proc.WaitForExit();
        }
    }
}
