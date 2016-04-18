using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nyx.Scheduler.Interfaces;

namespace Nyx.Scheduler.Tasks
{
    public class ImportTransactionsTask : ITask
    {
        public string State { get; private set; }

        public void Execute()
        {
            Console.WriteLine("Running import task for 15 seconds...");
            State = "Importing...";

            for (int i = 0; i < 100; i++)
            {
                System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(100));
            }

            State = "Imported";
            Console.WriteLine("Import task finished.");
        }

        public string Name => "ImportTransactions";
    }
}
