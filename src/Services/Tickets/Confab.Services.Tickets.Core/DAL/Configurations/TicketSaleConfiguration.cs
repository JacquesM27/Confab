using Confab.Services.Tickets.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Confab.Services.Tickets.Core.DAL.Configurations;

internal sealed class TicketSaleConfiguration : IEntityTypeConfiguration<TicketSale>
{
    public void Configure(EntityTypeBuilder<TicketSale> builder)
    {
    }
}