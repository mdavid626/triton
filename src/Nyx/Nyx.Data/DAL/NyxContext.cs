using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Nyx.Data.Models;

namespace Nyx.Data.DAL
{
    public class NyxContext : DbContext
    {
        public NyxContext() : base("DefaultConnection")
        {
            Database.SetInitializer<NyxContext>(null);
        }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<SchedulerItem> SchedulerItems { get; set; }

        public DbSet<SchemaVersions> SchemaVersions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}