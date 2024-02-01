using Confab.Modules.Agendas.Application.Agendas.DTO;
using Confab.Modules.Agendas.Application.Agendas.Queries;
using Confab.Modules.Agendas.Domain.Agendas.Entities;
using Confab.Modules.Agendas.Infrastructure.EF.Mappings;
using Confab.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace Confab.Modules.Agendas.Infrastructure.EF.Queries.Handlers;

internal sealed class GetAgendaTrackHandler(
    AgendasDbContext dbContext
    ) : IQueryHandler<GetAgendaTrack, AgendaTrackDto>
{
    private readonly DbSet<AgendaTrack> _agendaTracks = dbContext.AgendaTracks;
    public async Task<AgendaTrackDto> HandleAsync(GetAgendaTrack query)
    {
        var agendaTrack = await _agendaTracks
            .Include(a => a.Slots)
            .ThenInclude(a => (a as RegularAgendaSlot).AgendaItem)
            .ThenInclude(a => a.Speakers)
            .SingleOrDefaultAsync(a => a.Id == query.Id);

        return agendaTrack?.AsDto();
    }
}