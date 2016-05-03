using System;
using System.IO;
using System.Linq;
using Cadmus.Parametrizer.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cadmus.Parametrizer.Tests
{
    [TestClass]
    public class ConfigManagerTest
    {
        [TestMethod]
        public void TestSimpleSave()
        {
            var config = CreateTestConfig();
            ConfigManager.SaveConfig(config);
        }

        private Configuration CreateTestConfig(string name = "config.xml")
        {
            var config = new Configuration();
            var step = new Step() { Name = "Test" };
            step.Operations.Add(new RunOperation() { ExecutablePath = "notepad.exe", Title = "notepad" });
            config.Steps.Add(step);
            config.Parameters.Add(new Parameter()
            {
                Name = "TestParam",
                Description = "description",
                Editor = EditorOptions.Text,
                Value = "testvalue"
            });
            config.FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, name);
            return config;
        }

        [TestMethod]
        public void TestSimpleLoad()
        {
            var testConfig = CreateTestConfig();
            ConfigManager.SaveConfig(testConfig);
            ConfigManager.LoadConfig(testConfig.FilePath);
        }

        [TestMethod]
        public void TestComplexSave()
        {
            var baseConfig = CreateTestConfig("configbase.xml");
            var userConfig = new Configuration()
            {
                FilePath = Path.Combine(Path.GetDirectoryName(baseConfig.FilePath), "config.xml"),
                Parent = "configbase.xml"
            };
            baseConfig.ChildConfigurations.Add(userConfig);

            userConfig.Parameters.Add(new Parameter()
            {
                Name = "TestParam",
                Value = "uservalue",
                ValueComesFromConfiguration = userConfig
            });

            ConfigManager.SaveConfig(baseConfig);
            ConfigManager.SaveConfig(userConfig);

            var man = new ConfigManager(userConfig.FilePath);
            man.Load();
            var param = man.Config.Parameters.FirstOrDefault(p => p.Name == "TestParam");
            param.Value = "testnewvalue";
            man.Save();
            man.Load();
            param = man.Config.Parameters.FirstOrDefault(p => p.Name == "TestParam");
            Assert.IsTrue(param.Value == "testnewvalue");
        }

        [TestMethod]
        public void TestComplexLoad()
        {
            var baseConfig = CreateTestConfig("configbase.xml");
            var userConfig = new Configuration()
            {
                FilePath = Path.Combine(Path.GetDirectoryName(baseConfig.FilePath), "config.xml"),
                Parent = "configbase.xml"
            };
            baseConfig.ChildConfigurations.Add(userConfig);

            userConfig.Parameters.Add(new Parameter()
            {
                Name = "TestParam",
                Value = "uservalue",
                ValueComesFromConfiguration = userConfig
            });

            ConfigManager.SaveConfig(baseConfig);
            ConfigManager.SaveConfig(userConfig);

            var man = new ConfigManager(userConfig.FilePath);
            man.Load();
            var param = man.Config.Parameters.FirstOrDefault(p => p.Name == "TestParam");
            Assert.IsTrue(param.Value == "uservalue");
        }
    }
}
