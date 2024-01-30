using Confab.Modules.Agendas.Domain.Submissions.Entities;
using Confab.Modules.Agendas.Domain.Submissions.Repositories;
using Confab.Shared.Abstractions.Kernel.Types;
using Microsoft.EntityFrameworkCore;

namespace Confab.Modules.Agendas.Infrastructure.EF.Repositories;

internal sealed class SpeakerRepository(AgendasDbContext context) : ISpeakerRepository
{
    private readonly DbSet<Speaker> _speakers = context.Speakers;

    public Task<bool> ExistsAsync(AggregateId id)
        => _speakers.AnyAsync(x => x.Id.Equals(id));

    public async Task<IEnumerable<Speaker>> BrowseAsync(IEnumerable<AggregateId> ids)
        => await _speakers.Where(x => ids.Equals(x.Id)).ToListAsync();

    public async Task AddAsync(Speaker speaker)
    {
        await _speakers.AddAsync(speaker);
        await context.SaveChangesAsync();
    }
}