using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nyx.Scheduler.Framework;
using Nyx.Scheduler.Tasks;

namespace Nyx.Scheduler
{
    class Program
    {
        enum ReturnCode
        {
            Success = 0,
            ArgumentsError = 1,
            Exception = 2,
        }

        static int Main(string[] args)
        {
            try
            {
                var parser = new ApplicationArgumentsParser();
                var result = parser.Parse(args);

                if (result.HasErrors || result.AdditionalOptionsFound.Any())
                {
                    parser.ShowHelp();
                    return (int) ReturnCode.ArgumentsError;
                }

                ChooseOperation(parser);
                return (int) ReturnCode.Success;
            }
            catch (SchedulerException ex)
            {
                Console.Error.WriteLine(ex.Message);
                return (int) ReturnCode.Exception;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
                return (int) ReturnCode.Exception;
            }
        }

        static void ChooseOperation(ApplicationArgumentsParser parser)
        {
            if (parser.Arguments.Help)
            {
                parser.ShowHelp();
            }
            else if (parser.Arguments.Version)
            {
                parser.ShowVersion();
            }
            else if (parser.Arguments.Shutdown)
            {
                var client = new PipeClient();
                client.Shutdown();
            }
            else if (parser.Arguments.Schedule)
            {
                var scheduler = new NyxTaskScheduler();
                scheduler.RegisterTask(new ImportTransactionsTask());
                scheduler.RegisterTask(new CleanupTask());
                scheduler.Run(parser.Arguments.Force, parser.Arguments.UnLock);
            }
            else
            {
                parser.ShowHelp();
            }
        }
    }
}
