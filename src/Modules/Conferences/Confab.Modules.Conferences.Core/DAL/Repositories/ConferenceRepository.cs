using Confab.Modules.Conferences.Core.Entities;
using Confab.Modules.Conferences.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Confab.Modules.Conferences.Core.DAL.Repositories
{
    internal sealed class ConferenceRepository : IConferenceRepository
    {
        private readonly ConferencesDbContext _dbContext;
        private readonly DbSet<Conference> _conferences;  

        public ConferenceRepository(ConferencesDbContext dbContext)
        {
            _dbContext = dbContext;
            _conferences = _dbContext.Conferences;
        }

        public async Task AddAsync(Conference conference)
        {
            await _conferences.AddAsync(conference);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<Conference>> BrowseAsync()
            => await _conferences.Include(x => x.Host).ToListAsync();

        public async Task DeleteAsync(Conference conference)
        {
            _conferences.Remove(conference);
            await _dbContext.SaveChangesAsync();
        }

        public Task<Conference?> GetAsync(Guid id)
            => _conferences.Include(x => x.Host).SingleOrDefaultAsync(x => x.Id == id);

        public async Task UpdateAsync(Conference conference)
        {
            _conferences.Update(conference);
            await _dbContext.SaveChangesAsync();
        }
    }
}
