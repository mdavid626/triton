using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadmus.Foundation
{
    public class LogRouter
    {
        private static bool _isVerboseMode;

        public const string Success = "{{Success}}";
        public const string Warning = "{{Warning}}";
        public const string Error = "{{Error}}";
        public const string Header = "{{Header}}";
        public const string Verbose = "{{Verbose}}";
        public const string StartVerbose = "{{StartVerbose}}";
        public const string StopVerbose = "{{StopVerbose}}";

        public static void Route(ILogger logger, string msg)
        {
            if (!msg.IsNullOrEmpty())
            {
                if (msg.StartsWith(StartVerbose))
                    _isVerboseMode = true;
                else if (msg.StartsWith(StopVerbose))
                    _isVerboseMode = false;
                else
                {
                    if (_isVerboseMode)
                        logger.LogVerbose(msg);
                    else if (msg.StartsWith(Verbose))
                        logger.LogVerbose(msg.Substring(Verbose.Length));
                    else if (msg.StartsWith(Success))
                        logger.LogSuccess(msg.Substring(Success.Length));
                    else if (msg.StartsWith(Warning))
                        logger.LogWarning(msg.Substring(Warning.Length));
                    else if (msg.StartsWith(Error))
                        logger.LogError(msg.Substring(Error.Length));
                    else if (msg.StartsWith(Header))
                        logger.LogHeader(msg.Substring(Header.Length));
                    else
                        logger.LogInfo(msg);
                }
            }
            else
            {
                logger.LogInfo(msg);
            }
        }

        public static void StartVerboseMode()
        {
            _isVerboseMode = true;
        }

        public static void StopVerboseMode()
        {
            _isVerboseMode = false;
        }
    }
}
