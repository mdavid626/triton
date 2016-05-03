using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cadmus.Foundation;

namespace Cadmus.Parametrizer
{
    public class ConfigManager
    {
        public string FilePath { get; }

        public Configuration Config { get; protected set; }

        public ConfigManager(string filePath)
        {
            FilePath = filePath;
        }

        public Configuration Load()
        {
            Config = Load(FilePath);
            return Config;
        }

        public void Save()
        {
            Save(FilePath, Config);
        }

        public Configuration Load(string path)
        {
            var fullPath = Path.GetFullPath(path);
            var s = File.ReadAllText(fullPath);
            var param = s.Deserialize<Configuration>();
            return param;
        }

        public void Save(string path, Configuration config)
        {
            var s = config.Serialize();
            File.WriteAllText(path, s);
        }
    }
}
