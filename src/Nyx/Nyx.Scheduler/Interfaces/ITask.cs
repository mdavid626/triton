using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyx.Scheduler.Interfaces
{
    public interface ITask
    {
        void Execute();

        string State { get; }

        string Name { get; }
    }
}
