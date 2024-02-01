using Confab.Modules.Agendas.Application.Agendas.DTO;
using Confab.Modules.Agendas.Application.Agendas.Queries;
using Confab.Modules.Agendas.Domain.Agendas.Entities;
using Confab.Modules.Agendas.Infrastructure.EF.Mappings;
using Confab.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace Confab.Modules.Agendas.Infrastructure.EF.Queries.Handlers;

internal sealed class GetAgendaItemHandler(
    AgendasDbContext dbContext
    ) : IQueryHandler<GetAgendaItem, AgendaItemDto>
{
    private readonly DbSet<AgendaItem> _agendaItems = dbContext.AgendaItems;

    public async Task<AgendaItemDto> HandleAsync(GetAgendaItem query)
        => await _agendaItems
            .AsNoTracking()
            .Include(a => a.Speakers)
            .Where(a => a.Id == query.Id)
            .Select(a => a.AsDto())
            .FirstOrDefaultAsync();
}