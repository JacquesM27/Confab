using System.Collections;
using Confab.Modules.Agendas.Application.Agendas.DTO;
using Confab.Modules.Agendas.Application.Agendas.Queries;
using Confab.Modules.Agendas.Domain.Agendas.Entities;
using Confab.Modules.Agendas.Infrastructure.EF.Mappings;
using Confab.Shared.Abstractions.Queries;
using Confab.Shared.Abstractions.Storage;
using Microsoft.EntityFrameworkCore;

namespace Confab.Modules.Agendas.Infrastructure.EF.Queries.Handlers;

internal sealed class GetAgendaHandler(
    AgendasDbContext dbContext,
    IRequestStorage requestStorage
    ) : IQueryHandler<GetAgenda, IEnumerable<AgendaTrackDto>>
{
    private readonly DbSet<AgendaTrack> _agendaTracks = dbContext.AgendaTracks;
    
    public async Task<IEnumerable<AgendaTrackDto>> HandleAsync(GetAgenda query)
    {
        var storageDtos = requestStorage.Get<IEnumerable<AgendaTrackDto>>(GetStorageKey(query.ConferenceId));
        if (storageDtos is not null)
            return storageDtos;

        var agendaTracks = await _agendaTracks
            .Include(a => a.Slots)
            .ThenInclude(s => (s as RegularAgendaSlot).AgendaItem)
            .ThenInclude(a => a.Speakers)
            .Where(a => a.ConferenceId == query.ConferenceId)
            .ToListAsync();

        storageDtos = agendaTracks?.Select(a => a.AsDto());
        requestStorage.Set(GetStorageKey(query.ConferenceId), storageDtos, TimeSpan.FromMinutes(5));
        return storageDtos;
    }

    private static string GetStorageKey(Guid conferenceId) => $"agenda/{conferenceId}";
}