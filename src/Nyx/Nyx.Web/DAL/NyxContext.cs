using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Nyx.Web.Models;

namespace Nyx.Web.DAL
{
    public class NyxContext : DbContext
    {
        public NyxContext() : base("DefaultConnection")
        {
            Database.SetInitializer<NyxContext>(null);
        }

        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}