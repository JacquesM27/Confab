using System.Text.Json;
using Confab.Modules.Users.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Confab.Modules.Users.Core.DAL.Configuration;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(x => x.Claims)
            .HasConversion(x => JsonSerializer.Serialize(x, SerializerOptions),
                x => JsonSerializer.Deserialize<Dictionary<string, IEnumerable<string>>>(x, SerializerOptions));

        builder.Property(x => x.Claims).Metadata.SetValueComparer(
            new ValueComparer<Dictionary<string, IEnumerable<string>>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToDictionary(x => x.Key, x => x.Value)));
    }
    
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };
}