using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nyx.Scheduler.Interfaces;

namespace Nyx.Scheduler.Tasks
{
    public class CleanupTask : ITask
    {
        public string State { get; private set; }

        public void Execute()
        {
            Console.WriteLine("Running cleanup task for 15 seconds...");
            State = "Cleaning...";

            for (int i = 0; i < 100; i++)
            {
                System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(100));
            }

            State = "Cleaned";
            Console.WriteLine("Cleanup task finished.");
        }

        public string Name => "Cleanup";
    }
}
