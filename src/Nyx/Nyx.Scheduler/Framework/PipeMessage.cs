using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyx.Scheduler.Framework
{
    [Serializable]
    public class PipeMessage
    {
        public string Message { get; set; }

        public bool ShutdownRequest { get; set; }

        public bool ShuttedDown { get; set; }

        public bool Ack { get; set; }
    }
}
