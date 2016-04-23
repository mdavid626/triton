using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Nyx.Scheduler.Framework
{
    public class SchedulerException : Exception
    {
        public SchedulerException()
        {

        }

        public SchedulerException(string message) : base(message)
        {

        }

        public SchedulerException(string message, Exception innerException) : base(message, innerException)
        {

        }

        protected SchedulerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
