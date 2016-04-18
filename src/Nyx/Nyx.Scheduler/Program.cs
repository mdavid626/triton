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
        enum ExitResult
        {
            Success = 0,
            Failure = 1
        }

        static int Main(string[] args)
        {
            var scheduler = new NyxTaskScheduler();
            scheduler.RegisterTask(new ImportTransactionsTask());
            scheduler.RegisterTask(new CleanupTask());
            var result = scheduler.Run();
            return result ? (int)ExitResult.Success : (int)ExitResult.Failure;
        }
    }
}
