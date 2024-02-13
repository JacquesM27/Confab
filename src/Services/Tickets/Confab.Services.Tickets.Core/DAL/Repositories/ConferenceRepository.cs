using Confab.Services.Tickets.Core.Entities;
using Confab.Services.Tickets.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Confab.Services.Tickets.Core.DAL.Repositories;

internal sealed class ConferenceRepository : IConferenceRepository
{
    private readonly TicketsDbContext _dbContext;
    private readonly DbSet<Conference> _conferences;
    
    public ConferenceRepository(TicketsDbContext dbContext)
    {
        _dbContext = dbContext;
        _conferences = _dbContext.Conferences;
    }

    public Task<Conference?> GetAsync(Guid id)
        => _conferences.SingleOrDefaultAsync(x => x.Id == id);

    public async Task AddAsync(Conference conference)
    {
        await _conferences.AddAsync(conference);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Conference conference)
    {
        _conferences.Update(conference);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Conference conference)
    {
        _conferences.Remove(conference);
        await _dbContext.SaveChangesAsync();
    }
}