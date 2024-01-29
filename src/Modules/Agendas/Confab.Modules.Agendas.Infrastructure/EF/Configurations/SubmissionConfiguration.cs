using Confab.Modules.Agendas.Domain.Submissions.Entities;
using Confab.Shared.Abstractions.Kernel.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Confab.Modules.Agendas.Infrastructure.EF.Configurations;

internal sealed class SubmissionConfiguration : IEntityTypeConfiguration<Submission>
{
    public void Configure(EntityTypeBuilder<Submission> builder)
    {
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(x => x, y => new AggregateId(y));
        
        builder.Property(x => x.ConferenceId)
            .HasConversion(x => x.Value, y => new ConferenceId(y));

        builder.Property(x => x.Tags)
            .HasConversion(x => string.Join(',', x), x => x.Split(',', StringSplitOptions.None));
        builder.Property(x => x.Tags).Metadata.SetValueComparer(new ValueComparer<IEnumerable<string>>(
            (c1, c2) => c1.SequenceEqual(c2),
            c => c.Aggregate(0, (a, next) => HashCode.Combine(a, next.GetHashCode())),
            c => c.ToArray()));
        
        builder.Property(x => x.Version).IsConcurrencyToken();
    }
}