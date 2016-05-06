using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadmus.Foundation
{
    public abstract class LoggerBase : ILogger
    {
        public bool IsVerbose { get; set; }

        public abstract void LogInfo(string msg);

        public virtual void LogSuccess(string msg)
        {
            LogInfo(msg);
        }

        public virtual void LogWarning(string msg)
        {
            LogInfo(msg);
        }

        public virtual void LogError(string msg)
        {
            LogInfo(msg);
        }

        public virtual void LogVerbose(string msg)
        {
            if (IsVerbose)
                LogInfo(msg);
        }

        public virtual void LogHeader(string msg)
        {
            LogInfo(msg);
        }

        public virtual void StartVerbose()
        {
            LogRouter.StartVerboseMode();
        }

        public virtual void StopVerbose()
        {
            LogRouter.StopVerboseMode();
        }

        public virtual void Clear()
        {
            
        }
    }
}
