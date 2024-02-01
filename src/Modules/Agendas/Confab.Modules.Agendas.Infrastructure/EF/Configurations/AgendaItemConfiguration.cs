using Confab.Modules.Agendas.Domain.Agendas.Entities;
using Confab.Shared.Abstractions.Kernel.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Confab.Modules.Agendas.Infrastructure.EF.Configurations;

public sealed class AgendaItemConfiguration : IEntityTypeConfiguration<AgendaItem>
{
    public void Configure(EntityTypeBuilder<AgendaItem> builder)
    {
        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasConversion(x => x, 
                x => new AggregateId(x));

        builder
            .Property(x => x.ConferenceId)
            .HasConversion(x => x.Value, 
                x => new ConferenceId(x));

        builder
            .Property(x => x.Tags)
            .HasConversion(tags => string.Join(",", tags), 
                tags => tags.Split(",", StringSplitOptions.None));

        builder.Property(x => x.Version).IsConcurrencyToken();
        
        builder.Property(x => x.Tags).Metadata.SetValueComparer(
            new ValueComparer<IEnumerable<string>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode()))));

    }
}