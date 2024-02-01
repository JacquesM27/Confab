using Confab.Modules.Agendas.Domain.Agendas.Entities;
using Confab.Shared.Abstractions.Kernel.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Confab.Modules.Agendas.Infrastructure.EF.Configurations;

public sealed class AgendaTrackConfiguration : IEntityTypeConfiguration<AgendaTrack>
{
    public void Configure(EntityTypeBuilder<AgendaTrack> builder)
    {
        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasConversion(id => id, id => new AggregateId(id));

        builder
            .Property(x => x.ConferenceId)
            .HasConversion(id => id.Value, id => new ConferenceId(id));

        builder
            .HasMany(at => at.Slots)
            .WithOne(slot => slot.Track);

        builder
            .Property(at => at.Version)
            .IsConcurrencyToken();
    }
}