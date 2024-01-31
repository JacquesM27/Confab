using Confab.Modules.Agendas.Application.Agendas.Events;
using Confab.Modules.Agendas.Application.Agendas.Exceptions;
using Confab.Modules.Agendas.Domain.Agendas.Repositories;
using Confab.Modules.Agendas.Domain.Agendas.Services;
using Confab.Shared.Abstractions.Commands;
using Confab.Shared.Abstractions.Messaging;

namespace Confab.Modules.Agendas.Application.Agendas.Commands.Handlers;

internal sealed class AssignRegularAgendaSlotHandler(
    IAgendaTracksRepository repository,
    IAgendaTracksDomainService domainService,
    IMessageBroker messageBroker
    ) : ICommandHandler<AssignRegularAgendaSlot>
{
    public async Task HandleAsync(AssignRegularAgendaSlot command)
    {
        var agendaTrack = await repository.GetAsync(command.AgendaTrackId)
                          ?? throw new AgendaTrackNotFoundException(command.AgendaTrackId);

        await domainService.AssignAgendaItemAsync(agendaTrack, command.AgendaSlotId, command.AgendaItemId);

        await repository.UpdateAsync(agendaTrack);
        await messageBroker.PublishAsync(
            new AgendaItemAssignedToAgendaSlot(command.AgendaSlotId, command.AgendaItemId));

    }
}