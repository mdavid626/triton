using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cadmus.Foundation;
using Cadmus.Parametrizer.Options;

namespace Cadmus.Parametrizer
{
    public class ConfigManager
    {
        public string ConfigPath { get; protected set; }

        public Configuration Config { get; protected set; }

        public Dictionary<string, string> BaseValues { get; protected set; }

        public ConfigManager()
        {
            
        }

        public ConfigManager(string configPath)
        {
            ConfigPath = configPath;
        }

        public void Load()
        {
            string currentPath = ConfigPath;
            var configs = new List<Configuration>();

            while (File.Exists(currentPath))
            {
                var config = LoadConfig(currentPath);
                configs.Add(config);
                currentPath = GetParentConfigPath(config);
            }

            configs.Reverse();
            Config = configs.FirstOrDefault();
            BaseValues = new Dictionary<string, string>();

            if (Config != null)
            {
                Config.Parameters.Apply(p => BaseValues.Add(p.Name, p.Value));

                foreach (var config in configs.Skip(1))
                {
                    Config.MergeValues(config);
                }
            }
        }

        private string GetParentConfigPath(Configuration currentConfiguration)
        {
            var path = currentConfiguration.Parent;
            if (Path.IsPathRooted(path) || path.IsNullOrEmpty())
                return path;
            var dir = Path.GetDirectoryName(currentConfiguration.FilePath);
            return Path.Combine(dir, path);
        }

        public void Save()
        {
            var lastConfig = Config.ChildConfigurations.LastOrDefault();
            var changedConfigs = new List<Configuration>();

            foreach (var param in Config.Parameters.Where(p => p.Editor != EditorOptions.Hidden))
            {
                string baseValue;
                BaseValues.TryGetValue(param.Name, out baseValue);

                if (param.Value != baseValue)
                {
                    var configuration = param.ValueComesFromConfiguration;
                    if (configuration == null)
                        configuration = lastConfig;

                    var configParam = configuration?.Parameters.FirstOrDefault(p => p.Name == param.Name);
                    if (configParam == null && configuration != null)
                    {
                        configParam = new Parameter() { Name = param.Name };
                        configuration.Parameters.Add(configParam);
                    }

                    if (configParam != null && configParam.Value != param.Value)
                    {
                        configParam.Value = param.Value;
                        if (!changedConfigs.Contains(configuration))
                            changedConfigs.Add(configuration);
                    }
                }
            }

            foreach (var config in changedConfigs)
            {
                SaveConfig(config);
            }
        }

        public static Configuration LoadConfig(string path)
        {
            var fullPath = Path.GetFullPath(path);
            var s = File.ReadAllText(fullPath);
            var param = s.Deserialize<Configuration>();
            param.FilePath = path;
            return param;
        }

        public static void SaveConfig(Configuration config)
        {
            var s = config.Serialize();
            File.WriteAllText(config.FilePath, s);
        }
    }
}
