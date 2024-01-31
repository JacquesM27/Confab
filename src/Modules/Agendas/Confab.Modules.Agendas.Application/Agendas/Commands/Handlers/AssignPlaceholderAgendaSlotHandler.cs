using Confab.Modules.Agendas.Application.Agendas.Events;
using Confab.Modules.Agendas.Application.Agendas.Exceptions;
using Confab.Modules.Agendas.Domain.Agendas.Repositories;
using Confab.Shared.Abstractions.Commands;
using Confab.Shared.Abstractions.Messaging;

namespace Confab.Modules.Agendas.Application.Agendas.Commands.Handlers;

internal sealed class AssignPlaceholderAgendaSlotHandler(
    IAgendaTracksRepository repository,
    IMessageBroker messageBroker
    ) : ICommandHandler<AssignPlaceholderAgendaSlot>
{
    public async Task HandleAsync(AssignPlaceholderAgendaSlot command)
    {
        var agendaTrack = await repository.GetAsync(command.AgendaTrackId)
                          ?? throw new AgendaTrackNotFoundException(command.AgendaTrackId);
        
        agendaTrack.ChangeSlotPlaceholder(command.AgendaSlotId, command.Placeholder);

        await repository.UpdateAsync(agendaTrack);
        await messageBroker.PublishAsync(
            new PlaceholderAssignedToAgendaSlot(command.AgendaSlotId, command.Placeholder));
    }
}