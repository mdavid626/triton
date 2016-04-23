using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nyx.Scheduler.Interfaces
{
    public interface ITask
    {
        void Execute(CancellationToken cancelToken);

        string State { get; }

        string Name { get; }
    }
}
