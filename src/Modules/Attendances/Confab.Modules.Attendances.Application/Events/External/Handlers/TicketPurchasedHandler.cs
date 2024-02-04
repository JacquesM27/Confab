using Confab.Modules.Attendances.Domain.Entities;
using Confab.Modules.Attendances.Domain.Repositories;
using Confab.Shared.Abstractions.Events;
using Microsoft.Extensions.Logging;

namespace Confab.Modules.Attendances.Application.Events.External.Handlers;

internal sealed class TicketPurchasedHandler(
    IParticipantsRepository participantsRepository,
    ILogger<TicketPurchasedHandler> logger
    ) : IEventHandler<TicketPurchased>
{
    public async Task HandleAsync(TicketPurchased @event)
    {
        var participant = await participantsRepository.GetAsync(@event.ConferenceId, @event.UserId);
        if (participant is not null)
            return;

        participant = new Participant(Guid.NewGuid(), @event.ConferenceId, @event.UserId);
        await participantsRepository.AddAsync(participant);
        logger.LogInformation($"Added a participant with ID: '{participant.Id}' " +
                              $"for conference: '{participant.ConferenceId}', user: '{participant.UserId}'.");
    }
}