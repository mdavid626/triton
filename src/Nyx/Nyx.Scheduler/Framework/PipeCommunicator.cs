using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyx.Scheduler.Framework
{
    public class PipeCommunicator
    {
        public PipeCommunicator()
        {
            PipeName = GeneratePipeName();
            
        }

        public string PipeName { get; }

        protected string GeneratePipeName()
        {
            return UniqueIdGenerator.Generate("SchedulerPipe_");
        }
    }
}
