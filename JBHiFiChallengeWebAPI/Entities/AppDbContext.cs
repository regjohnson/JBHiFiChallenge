using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace JBHiFiChallengeWebAPI.Entities
{
    public class AppDbContext : DbContext
    {
        public DbSet<KeyUsage> KeyUsages { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<KeyUsage>();
        }
    }
}
