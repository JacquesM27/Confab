using Confab.Modules.Tickets.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Confab.Modules.Tickets.Core.DAL;

public class TicketsDbContext(
    DbContextOptions<TicketsDbContext> options
    ) : DbContext(options)
{
    public DbSet<Conference> Conferences { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<TicketSale> TicketSales { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("tickets");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}