using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadmus.Foundation
{
    public class MultiLogger : ILogger
    {
        private readonly List<ILogger> _loggers = new List<ILogger>();

        public void Add(ILogger logger)
        {
            _loggers.Add(logger);
        }

        public void LogInfo(string msg)
        {
            _loggers.Apply(o => o.LogInfo(msg));
        }

        public void LogSuccess(string msg)
        {
            _loggers.Apply(o => o.LogSuccess(msg));
        }

        public void LogWarning(string msg)
        {
            _loggers.Apply(o => o.LogWarning(msg));
        }

        public void LogError(string msg)
        {
            _loggers.Apply(o => o.LogError(msg));
        }

        public void LogVerbose(string msg)
        {
            _loggers.Apply(o => o.LogVerbose(msg));
        }

        public void LogHeader(string msg)
        {
            _loggers.Apply(o => o.LogHeader(msg));
        }

        public void StartVerbose()
        {
            _loggers.Apply(o => o.StartVerbose());
        }

        public void StopVerbose()
        {
            _loggers.Apply(o => o.StopVerbose());
        }

        public void Clear()
        {
            _loggers.Apply(o => o.Clear());
        }
    }
}
