using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nyx.Scheduler.Interfaces;

namespace Nyx.Scheduler.Framework
{
    public class ApplicationArguments : IApplicationArguments
    {
        public bool Shutdown { get; set; }

        public bool Schedule { get; set; }

        public bool Version { get; set; }

        public bool Force { get; set; }

        public bool Help { get; set; }

        public bool UnLock { get; set; }
    }
}
