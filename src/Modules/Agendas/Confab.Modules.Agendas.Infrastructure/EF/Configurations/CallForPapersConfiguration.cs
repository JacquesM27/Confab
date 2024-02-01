using Confab.Modules.Agendas.Domain.CallForPapers.Entities;
using Confab.Shared.Abstractions.Kernel.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Confab.Modules.Agendas.Infrastructure.EF.Configurations;

public sealed class CallForPapersConfiguration : IEntityTypeConfiguration<CallForPapers>
{
    public void Configure(EntityTypeBuilder<CallForPapers> builder)
    {
        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasConversion(id => id, id => new AggregateId(id));

        builder
            .Property(x => x.ConferenceId)
            .HasConversion(id => id.Value, id => new ConferenceId(id));

        builder
            .Property(x => x.Version)
            .IsConcurrencyToken();
    }
}