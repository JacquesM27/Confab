using Confab.Modules.Agendas.Application.Agendas.DTO;
using Confab.Modules.Agendas.Application.Agendas.Queries;
using Confab.Modules.Agendas.Domain.Agendas.Entities;
using Confab.Modules.Agendas.Infrastructure.EF.Mappings;
using Confab.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace Confab.Modules.Agendas.Infrastructure.EF.Queries.Handlers;

internal sealed class GetRegularAgendaSlotHandler(
    AgendasDbContext dbContext
    ) : IQueryHandler<GetRegularAgendaSlot, RegularAgendaSlotDto>
{
    private readonly DbSet<AgendaSlot> _agendaSlots = dbContext.AgendaSlots;
    
    public async Task<RegularAgendaSlotDto> HandleAsync(GetRegularAgendaSlot query)
    {
        var slot = await _agendaSlots
            .OfType<RegularAgendaSlot>()
            .Include(r => r.AgendaItem)
            .ThenInclude(r => r.Speakers)
            .SingleOrDefaultAsync(r => r.AgendaItem.Id == query.AgendaItemId);

        return slot?.AsDto();
    }
}