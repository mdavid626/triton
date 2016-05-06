using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadmus.Foundation
{
    public interface ILogger
    {
        void LogInfo(string msg);

        void LogSuccess(string msg);

        void LogWarning(string msg);

        void LogError(string msg);

        void LogVerbose(string msg);

        void LogHeader(string msg);

        void StartVerbose();

        void StopVerbose();

        void Clear();
    }
}
