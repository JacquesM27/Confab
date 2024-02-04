using Confab.Modules.Agendas.Application.Agendas.Events;
using Confab.Modules.Agendas.Application.Agendas.Exceptions;
using Confab.Modules.Agendas.Application.Agendas.Types;
using Confab.Modules.Agendas.Domain.Agendas.Repositories;
using Confab.Shared.Abstractions.Commands;
using Confab.Shared.Abstractions.Messaging;

namespace Confab.Modules.Agendas.Application.Agendas.Commands.Handlers;

internal sealed class CreateAgendaSlotHandler(
    IAgendaTracksRepository repository,
    IMessageBroker messageBroker
    ) : ICommandHandler<CreateAgendaSlot>
{
    public async Task HandleAsync(CreateAgendaSlot command)
    {
        var agendaTrack = await repository.GetAsync(command.AgendaTrackId)
                          ?? throw new AgendaTrackNotFoundException(command.AgendaTrackId);

        if (command.Type == AgendaSlotType.Regular)
            agendaTrack.AddRegularSlot(command.Id, command.From, command.To, command.ParticipantsLimit);
        else if (command.Type == AgendaSlotType.Placeholder) 
            agendaTrack.AddPlaceholderSlot(command.Id, command.From, command.To);

        await repository.UpdateAsync(agendaTrack);
        await messageBroker.PublishAsync(new AgendaTrackUpdated(agendaTrack.Id));
    }
}