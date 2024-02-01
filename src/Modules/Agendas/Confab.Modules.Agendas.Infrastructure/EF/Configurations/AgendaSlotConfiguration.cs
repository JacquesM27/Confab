using Confab.Modules.Agendas.Application.Agendas.Types;
using Confab.Modules.Agendas.Domain.Agendas.Entities;
using Confab.Shared.Abstractions.Kernel.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Confab.Modules.Agendas.Infrastructure.EF.Configurations;

public sealed class AgendaSlotConfiguration : IEntityTypeConfiguration<AgendaSlot>
{
    public void Configure(EntityTypeBuilder<AgendaSlot> builder)
    {
        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasConversion(x => x.Value, x => new EntityId(x));

        builder
            .HasDiscriminator<string>("Type")
            .HasValue<PlaceholderAgendaSlot>(AgendaSlotType.Placeholder)
            .HasValue<RegularAgendaSlot>(AgendaSlotType.Regular);
    }
}