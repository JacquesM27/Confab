using Confab.Modules.Agendas.Domain.Agendas.Entities;
using Confab.Modules.Agendas.Domain.Agendas.Repositories;
using Confab.Shared.Abstractions.Kernel.Types;
using Microsoft.EntityFrameworkCore;

namespace Confab.Modules.Agendas.Infrastructure.EF.Repositories;

internal sealed class AgendaTracksRepository(
    AgendasDbContext context
    ) : IAgendaTracksRepository
{
    private readonly DbSet<AgendaTrack> _agendaTracks = context.AgendaTracks;

    public async Task<AgendaTrack> GetAsync(AggregateId id)
        => await _agendaTracks
            .Include(a => a.Slots)
            .SingleOrDefaultAsync(a => a.Id == id);

    public async Task<IEnumerable<AgendaTrack>> BrowseAsync(ConferenceId conferenceId)
        => await _agendaTracks
            .AsNoTracking()
            .Include(a => a.Slots)
            .Where(a => a.ConferenceId == conferenceId)
            .ToListAsync();

    public async Task<bool> ExistsAsync(AggregateId id)
        => await _agendaTracks.AnyAsync(a => a.Id == id);

    public async Task AddAsync(AgendaTrack agendaTrack)
    {
        await _agendaTracks.AddAsync(agendaTrack);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(AgendaTrack agendaTrack)
    {
        _agendaTracks.Update(agendaTrack);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(AgendaTrack agendaTrack)
    {
        _agendaTracks.Remove(agendaTrack);
        await context.SaveChangesAsync();
    }
}