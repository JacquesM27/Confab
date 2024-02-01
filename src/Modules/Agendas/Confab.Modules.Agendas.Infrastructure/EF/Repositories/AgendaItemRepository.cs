using Confab.Modules.Agendas.Domain.Agendas.Entities;
using Confab.Modules.Agendas.Domain.Agendas.Repositories;
using Confab.Shared.Abstractions.Kernel.Types;
using Microsoft.EntityFrameworkCore;

namespace Confab.Modules.Agendas.Infrastructure.EF.Repositories;

internal sealed class AgendaItemRepository(
    AgendasDbContext context
    ) : IAgendaItemRepository
{
    private readonly DbSet<AgendaItem> _agendaItems = context.AgendaItems;
    
    public async Task<IEnumerable<AgendaItem>> BrowseAsync(IEnumerable<SpeakerId> speakerIds)
    {
        var ids = speakerIds.Select(id => (Guid)id).ToList();
        return await _agendaItems
            .AsNoTracking()
            .Include(a => a.Speakers)
            .Where(a => a.Speakers.Any(s => ids.Contains(s.Id)))
            .ToListAsync();
    }

    public async Task<AgendaItem> GetAsync(AggregateId id)
        => await _agendaItems
            .Include(a => a.Speakers)
            .Include(a => a.AgendaSlot)
            .SingleOrDefaultAsync(a => a.Id == id);

    public async Task AddAsync(AgendaItem agendaItem)
    {
        context.AttachRange(agendaItem.Speakers);
        await _agendaItems.AddAsync(agendaItem);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(AgendaItem agendaItem)
    {
        _agendaItems.Update(agendaItem);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(AgendaItem agendaItem)
    {
        _agendaItems.Remove(agendaItem);
        await context.SaveChangesAsync();
    }
}