using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.VisualStudio.Shell;

namespace Hector.Framework
{
    public class DbVersionCreator
    {
        public const string ScriptsFolder = "Scripts";
        public const string DbUpProjectName = "DbUp";
        public const string DbProjKind = "{00d1a9c2-b5f0-4af3-8072-f6c62b433612}";
        public const string DbProjInsertFile = @"dbo\PostDeployment\InsertSchemaVersions.sql";

        public void Create(string dbVersionDescription)
        {
            var dbProj = FindDbProj();
            var dbUpProj = FindDbUpProject();

            var scriptsFolder = dbUpProj.ProjectItems.OfType<ProjectItem>().FirstOrDefault(i => i.Name == ScriptsFolder);
            var folderPath = Path.Combine(Path.GetDirectoryName(dbUpProj.FullName), ScriptsFolder);

            var scriptName = GetNextScriptName(folderPath, dbVersionDescription);
            var fileName = Path.Combine(folderPath, scriptName);
            var fs = File.Create(fileName);
            fs.Close();

            var item = scriptsFolder?.ProjectItems.AddFromFile(fileName);
            SetBuildAction(item);
            DTE.ItemOperations.OpenFile(fileName);

            var name = $"{dbUpProj.Name}.{ScriptsFolder}.{scriptName}";
            var insertStatement = CreateInsertStatement(name);
            var insertFile = Path.Combine(Path.GetDirectoryName(dbProj.FullName), DbProjInsertFile);
            File.AppendAllText(insertFile, insertStatement);
        }

        public DTE DTE => (DTE)Package.GetGlobalService(typeof(DTE));

        private string CreateInsertStatement(string scriptName)
        {
            return "insert into SchemaVersions (ScriptName, Applied) " +
                   $"values('{scriptName}', getdate())\r\n";
        }

        private string GetNextScriptName(string folder, string description)
        {
            if (string.IsNullOrEmpty(description))
                description = "script";

            var fileName = Directory.GetFiles(folder).OrderBy(f => f).LastOrDefault();
            if (string.IsNullOrEmpty(fileName))
                fileName = "Script0000_init.sql";
            else
                fileName = Path.GetFileName(fileName);

            var versionNumber = fileName.Split('_')[0].Replace("Script", "");
            int number;
            if (Int32.TryParse(versionNumber, out number))
            {
                return $"Script{number + 1:0000}_{description}.sql";
            }

            return "Script.sql";
        }

        private void SetBuildAction(ProjectItem item)
        {
            var prop = item.Properties.OfType<Property>().FirstOrDefault(p => p.Name == "BuildAction");
            if (prop != null)
                prop.Value = 3; // Embedded Resource
        }

        private Project FindDbProj()
        {
            return DTE.Solution.Cast<Project>().FirstOrDefault(p => p.Kind == DbProjKind);
        }

        private Project FindDbUpProject()
        {
            return DTE.Solution.Cast<Project>().FirstOrDefault(p => p.Name.Contains(DbUpProjectName));
        }
    }
}
