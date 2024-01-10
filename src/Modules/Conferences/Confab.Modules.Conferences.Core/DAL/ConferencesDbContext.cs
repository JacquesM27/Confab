using Confab.Modules.Conferences.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Confab.Modules.Conferences.Core.DAL
{
    internal sealed class ConferencesDbContext(DbContextOptions<ConferencesDbContext> options) 
        : DbContext(options)
    {
        public DbSet<Conference> Conferences { get; set; }
        public DbSet<Host> Hosts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("conferences");
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
