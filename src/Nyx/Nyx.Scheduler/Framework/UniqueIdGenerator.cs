using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Cadmus.Foundation;
using Nyx.Data.DAL;

namespace Nyx.Scheduler.Framework
{
    public class UniqueIdGenerator
    {
        private static Guid Id;

        private static Guid GetDatabaseId()
        {
            using (var db = new NyxContext())
            {
                var info = db.DatabaseInfos.First();
                return info.DatabaseId;
            }
        }

        public static string Generate(string prefix)
        {
            if (Id == Guid.Empty)
                Id = GetDatabaseId();
            return prefix + Id;
        }
    }
}
