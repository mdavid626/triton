using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Nyx.Scheduler.Interfaces
{
    public interface IApplicationArguments
    {
        bool Shutdown { get; set; }

        bool Schedule { get; set; }

        bool Version { get; set; }

        bool Force { get; set; }

        bool Help { get; set; }

        bool UnLock { get; set; }
    }
}
