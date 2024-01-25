using Confab.Modules.Speakers.Core.Entities;
using Confab.Modules.Speakers.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Confab.Modules.Speakers.Core.DAL.Repositories;

internal sealed class SpeakerRepository : ISpeakerRepository
{
    private readonly SpeakersDbContext _dbContext;
    private readonly DbSet<Speaker> _speakers;

    public SpeakerRepository(SpeakersDbContext dbContext)
    {
        _dbContext = dbContext;
        _speakers = _dbContext.Speakers;
    }

    public Task<Speaker?> GetAsync(Guid id)
        => _speakers.SingleOrDefaultAsync(x => x.Id == id);

    public async Task<IReadOnlyList<Speaker>> BrowseAsync()
        => await _speakers.ToListAsync();

    public async Task AddAsync(Speaker speaker)
    {
        await _speakers.AddAsync(speaker);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Speaker speaker)
    {
        _speakers.Update(speaker);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
        => await _speakers.AnyAsync(x => x.Id == id);
}