using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadmus.Foundation
{
    public interface ILogger
    {
        void Log(string msg);

        void Log(string msg, ConsoleColor color);

        void LogLine(string msg);

        void LogLine(string msg, ConsoleColor color);

        void LogSuccess(string msg);

        void LogWarning(string msg);

        void LogError(string msg);

        void Clear();
    }
}
