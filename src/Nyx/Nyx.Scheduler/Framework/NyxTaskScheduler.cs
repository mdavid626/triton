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

        public void RegisterTask(ITask task)
        {
            _tasks.Add(task);
        }

        public bool Run()
        {
            var plannedTasks = GetPlannedTasks();

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

        private ITask[] GetPlannedTasks()
        {
            var now = DateTime.Now;

            var items = _db.SchedulerItems.ToArray()
                .Where(t => (t.LastRun == null || t.LastRun.Value.AddMinutes(t.Interval) < now) && !t.Locked)
                .Select(t => t.Name);

            return _tasks.Where(t => items.Contains(t.Name)).ToArray();
        }

        private bool RunTask(ITask task)
        {
            try
            {
                Console.WriteLine("Starting task: " + task.Name);
                Lock(task);
                task.Execute();
                UnLock(task);
                return true;
            }
            catch (Exception ex)
            {
                // ReportError
            }
            finally
            {
                UnLock(task);
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
                    item.State = "Running...";
                    db.SaveChanges();
                }
            }
        }

        private void UnLock(ITask task)
        {
            using (var db = new NyxContext())
            {
                var item = db.SchedulerItems.FirstOrDefault(t => t.Name == task.Name);
                if (item != null)
                {
                    item.Locked = false;
                    item.LastRun = DateTime.Now;
                    item.NextRun = DateTime.Now.AddMinutes(item.Interval);
                    item.State = task.State;
                    db.SaveChanges();
                }
            }
        }
    }
}
