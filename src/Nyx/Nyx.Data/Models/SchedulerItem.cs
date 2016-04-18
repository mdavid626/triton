using System;
using System.ComponentModel.DataAnnotations;

namespace Nyx.Data.Models
{
    public class SchedulerItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Interval { get; set; }

        public string Title { get; set; }

        public DateTime? LastRun { get; set; }

        public string State { get; set; }

        public DateTime? NextRun { get; set; }

        public bool Locked { get; set;}

        public int LockValidTime { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}