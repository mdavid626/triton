using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nyx.Data.DAL;
using Nyx.Data.Models;

namespace Nyx.Web.Models
{
    public class SettingsViewModel
    {
        private readonly NyxContext _db;

        public SettingsViewModel(NyxContext db)
        {
            _db = db;
        }

        public IEnumerable<SchemaVersions> SchemaVersions { get; set; }

        public IEnumerable<SchedulerItem> SchedulerItems { get; set; }

        public void Load()
        {
            SchemaVersions = _db.SchemaVersions.OrderByDescending(s => s.Applied).ToList();
            SchedulerItems = _db.SchedulerItems.ToList();
        }
    }
}