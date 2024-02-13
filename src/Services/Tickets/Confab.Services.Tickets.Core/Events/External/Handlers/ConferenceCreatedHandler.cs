using Confab.Services.Tickets.Core.Entities;
using Confab.Services.Tickets.Core.Repositories;
using Confab.Shared.Abstractions.Events;
using Microsoft.Extensions.Logging;

namespace Confab.Services.Tickets.Core.Events.External.Handlers;

internal sealed class ConferenceCreatedHandler(
    IConferenceRepository conferenceRepository,
    ILogger<ConferenceCreatedHandler> logger) : IEventHandler<ConferenceCreated>
{
    public async Task HandleAsync(ConferenceCreated @event)
    {
        var conference = new Conference()
        {
            Id = @event.Id,
            Name = @event.Name,
            ParticipantsLimit = @event.ParticipantsLimit,
            From = @event.From,
            To = @event.To
        };

        await conferenceRepository.AddAsync(conference);
        logger.LogInformation($"Added a conference with ID: '{conference.Id}'.");
    }
}