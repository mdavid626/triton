using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Nyx.Scheduler.Interfaces;

namespace Nyx.Scheduler.Tasks
{
    public class CleanupTask : ITask
    {
        public void Execute(CancellationToken cancelToken)
        {
            Console.WriteLine("Running cleanup task for 13 seconds...");
            State = "Cleaning...";

            for (int i = 0; i < 130; i++)
            {
                System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(100));
                if (cancelToken.IsCancellationRequested)
                    break;
            }

            State = cancelToken.IsCancellationRequested ? "Canceled" : "Finished";
            Console.WriteLine("Cleanup task finished.");
        }

        public string State { get; private set; }

        public string Name => "Cleanup";
    }
}
