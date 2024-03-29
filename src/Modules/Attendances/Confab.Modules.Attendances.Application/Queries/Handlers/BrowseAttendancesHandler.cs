﻿using Confab.Modules.Attendances.Application.Clients.Agendas;
using Confab.Modules.Attendances.Application.Clients.Agendas.DTO;
using Confab.Modules.Attendances.Application.DTO;
using Confab.Modules.Attendances.Domain.Repositories;
using Confab.Shared.Abstractions.Queries;

namespace Confab.Modules.Attendances.Application.Queries.Handlers;

internal sealed class BrowseAttendancesHandler(
    IParticipantsRepository participantsRepository,
    IAgendasApiClient agendasApiClient
    ) : IQueryHandler<BrowseAttendances, IReadOnlyList<AttendanceDto>>
{
    public async Task<IReadOnlyList<AttendanceDto>> HandleAsync(BrowseAttendances query)
    {
        var participant = await participantsRepository.GetAsync(query.ConferenceId, query.UserId);
        if (participant is null)
            return null;

        var attendances = new List<AttendanceDto>();
        var tracks = await agendasApiClient.GetAgendaAsync(query.ConferenceId);
        var slots = tracks
            .SelectMany(x => x.Slots.OfType<RegularAgendaSlotDto>()).ToArray();
        
        foreach (var attendance in participant.Attendances)
        {
            var slot = slots.Single(x => x.AgendaItem.Id == attendance.AttendableEventId);
            attendances.Add(new AttendanceDto()
            {
                ConferenceId = query.ConferenceId,
                EventId = slot.Id,
                From = attendance.From,
                To = attendance.To,
                Title = slot.AgendaItem.Title,
                Description = slot.AgendaItem.Description,
                Level = slot.AgendaItem.Level
            });
        }

        return attendances;
    }
}