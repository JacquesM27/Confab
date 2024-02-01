using System.Collections;
using Confab.Modules.Agendas.Application.Agendas.DTO;
using Confab.Modules.Agendas.Application.Agendas.Queries;
using Confab.Modules.Agendas.Domain.Agendas.Entities;
using Confab.Modules.Agendas.Infrastructure.EF.Mappings;
using Confab.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace Confab.Modules.Agendas.Infrastructure.EF.Queries.Handlers;

internal sealed class BrowseAgendaItemsHandler(
    AgendasDbContext dbContext
    ) : IQueryHandler<BrowseAgendaItems, IEnumerable<AgendaItemDto>>
{
    private readonly DbSet<AgendaItem> _agendaItems = dbContext.AgendaItems;

    public async Task<IEnumerable<AgendaItemDto>> HandleAsync(BrowseAgendaItems query)
        => await _agendaItems
            .AsNoTracking()
            .Include(a => a.Speakers)
            .Where(a => a.ConferenceId == query.ConferenceId)
            .Select(a => a.AsDto())
            .ToListAsync();
}