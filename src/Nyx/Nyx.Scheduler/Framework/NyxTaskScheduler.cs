using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nyx.Data.DAL;
using Nyx.Scheduler.Interfaces;

namespace Nyx.Scheduler.Framework
{
    public class NyxTaskScheduler
    {
        private readonly NyxContext _db = new NyxContext();
        private readonly IList<ITask> _tasks = new List<ITask>();
        private readonly PipeServer _pipeCommunicator;

        public NyxTaskScheduler()
        {
            _pipeCommunicator = new PipeServer();
        }

        public void RegisterTask(ITask task)
        {
            _tasks.Add(task);
        }

        public bool Run(bool force = false, bool unlock = false)
        {
            using (var si = new InstanceChecker())
            {
                if (si.SingleInstance)
                {
                    using (_pipeCommunicator.Listen())
                    {
                        var plannedTasks = GetPlannedTasks(force, unlock);

                        if (plannedTasks.Any())
                            Console.WriteLine("Planned tasks: " + String.Join(", ", plannedTasks.Select(t => t.Name)));
                        else
                            Console.WriteLine("No planned tasks.");

                        var tasks = new List<Task<bool>>();

                        foreach (var task in plannedTasks)
                        {
                            var x = Task.Run(() => RunTask(task));
                            tasks.Add(x);
                        }

                        Task.WaitAll(tasks.ToArray());
                        return tasks.Any(t => !t.Result);
                    }
                }
                else
                {
                    throw new SchedulerException("Another instance of scheduler is already running.");
                }
            }
        }

        private ITask[] GetPlannedTasks(bool force, bool unlock)
        {
            var now = DateTime.Now;

            var items = _db.SchedulerItems.ToArray()
                .Where(t => (force || t.LastRun == null || t.LastRun.Value.AddMinutes(t.Interval) < now) && 
                            (unlock || !t.Locked || t.Locked && t.LastRun != null && t.LastRun.Value.AddMinutes(t.LockValidTime) < now))
                .Select(t => t.Name);

            return _tasks.Where(t => items.Contains(t.Name)).ToArray();
        }

        private bool RunTask(ITask task)
        {
            try
            {
                Console.WriteLine("Starting task: " + task.Name);
                Lock(task);
                task.Execute(_pipeCommunicator.CancelToken);
                UnLock(task);
                return true;
            }
            catch (Exception ex)
            {
                UnLock(task, ex);
            }
            return false;
        }

        private void Lock(ITask task)
        {
            using (var db = new NyxContext())
            {
                var item = db.SchedulerItems.FirstOrDefault(t => t.Name == task.Name);
                if (item != null)
                {
                    item.Locked = true;
                    item.LastRun = DateTime.Now;
                    item.State = "Running...";
                    db.SaveChanges();
                }
            }
        }

        private void UnLock(ITask task, Exception exception)
        {
            UnLock(task, exception.ToString());
        }

        private void UnLock(ITask task, string state = null)
        {
            using (var db = new NyxContext())
            {
                var item = db.SchedulerItems.FirstOrDefault(t => t.Name == task.Name);
                if (item != null)
                {
                    item.Locked = false;
                    item.NextRun = DateTime.Now.AddMinutes(item.Interval);
                    item.State = state ?? task.State;
                    db.SaveChanges();
                }
            }
        }
    }
}
