using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nyx.Scheduler.Framework
{
    public class InstanceChecker : IDisposable
    {
        private Mutex _mutex;

        public InstanceChecker()
        {
            _mutex = CreateMutex();
            WaitMutex();
        }

        public bool SingleInstance { get; private set; }

        private Mutex CreateMutex()
        {
            var name = UniqueIdGenerator.Generate("SchedulerMutex_");
            //bool mutexWasCreated;
            var m = new Mutex(true, name/*, out mutexWasCreated*/);
            //if (!mutexWasCreated)
            //    throw new Exception("Can not create mutex.");
            return m;
        }

        private void WaitMutex()
        {
            if (_mutex.WaitOne(TimeSpan.Zero, true))
            {
                SingleInstance = true;
            }
        }

        public void Dispose()
        {
            if (SingleInstance)
                _mutex.ReleaseMutex();
        }
    }
}
