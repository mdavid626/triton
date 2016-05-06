using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Cadmus.Foundation
{
    public class FileLogger : LoggerBase
    {
        private readonly object _fileLock = new object();

        public string FilePath { get; protected set; }

        public FileLogger(string filePath)
        {
            FilePath = filePath;
            IsVerbose = true;
            Clear();
        }

        public override void LogInfo(string msg)
        {
            lock (_fileLock)
            {
                File.AppendAllText(FilePath, msg + Environment.NewLine);
            }
        }

        public override void Clear()
        {
            File.WriteAllText(FilePath, String.Empty);
        }
    }
}
