using Confab.Modules.Agendas.Domain.CallForPapers.Entities;
using Confab.Modules.Agendas.Domain.Submissions.Entities;
using Microsoft.EntityFrameworkCore;

namespace Confab.Modules.Agendas.Infrastructure.EF;

internal sealed class AgendasDbContext(DbContextOptions<AgendasDbContext> options) 
    : DbContext(options)
{
    public DbSet<Speaker> Speakers { get; set; }
    public DbSet<Submission> Submissions { get; set; }
    public DbSet<CallForPapers> CallForPapers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("agendas");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}