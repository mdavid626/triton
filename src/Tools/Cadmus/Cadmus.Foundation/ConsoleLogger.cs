using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadmus.Foundation
{
    public class ConsoleLogger : LoggerBase
    {
        public ConsoleLogger()
        {
            IsVerbose = true;
        }

        private void LogInternal(string msg, ConsoleColor? color = null, bool newLine = false)
        {
            var endOfLine = newLine ? Environment.NewLine : String.Empty;

            var currentColor = Console.ForegroundColor;
            if (color.HasValue)
                Console.ForegroundColor = color.Value;

            Console.Write(msg + endOfLine);

            if (color.HasValue)
                Console.ForegroundColor = currentColor;
        }

        public override void LogInfo(string msg)
        {
            LogInternal(msg, newLine:true);
        }

        public void LogInfo(string msg, ConsoleColor color)
        {
            LogInternal(msg, color, newLine: true);
        }

        public override void LogSuccess(string msg)
        {
            if (!Console.IsOutputRedirected)
                LogInfo(msg, ConsoleColor.Green);
            else
                LogInfo(LogRouter.Success + msg);
        }

        public override void LogWarning(string msg)
        {
            if (!Console.IsOutputRedirected)
                LogInfo(msg, ConsoleColor.Yellow);
            else
                LogInfo(LogRouter.Warning + msg);
        }

        public override void LogError(string msg)
        {
            Console.Error.WriteLine(msg);
        }

        public override void LogHeader(string msg)
        {
            if (!Console.IsOutputRedirected)
                LogInfo(msg, ConsoleColor.Cyan);
            else
                LogInfo(LogRouter.Header + msg);
        }

        public override void LogVerbose(string msg)
        {
            if (IsVerbose)
            {
                if (!Console.IsOutputRedirected)
                    LogInfo(msg);
                else
                    LogInfo(LogRouter.Verbose + msg);
            }
        }

        public override void StartVerbose()
        {
            LogInfo(LogRouter.StartVerbose);
        }

        public override void StopVerbose()
        {
            LogInfo(LogRouter.StopVerbose);
        }

        public override void Clear()
        {
            Console.Clear();
        }
    }
}
