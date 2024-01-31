using Confab.Modules.Agendas.Application.Agendas.Events;
using Confab.Modules.Agendas.Application.Agendas.Exceptions;
using Confab.Modules.Agendas.Domain.Agendas.Repositories;
using Confab.Shared.Abstractions.Commands;
using Confab.Shared.Abstractions.Messaging;

namespace Confab.Modules.Agendas.Application.Agendas.Commands.Handlers;

internal sealed class ChangeAgendaTrackNameHandler(
    IAgendaTracksRepository repository,
    IMessageBroker messageBroker
    ) : ICommandHandler<ChangeAgendaTrackName>
{
    public async Task HandleAsync(ChangeAgendaTrackName command)
    {
        var agendaTrack = await repository.GetAsync(command.Id)
                          ?? throw new AgendaTrackNotFoundException(command.Id);
        
        agendaTrack.ChangeName(command.Name);

        await repository.UpdateAsync(agendaTrack);
        await messageBroker.PublishAsync(new AgendaTrackUpdated(agendaTrack.Id));
    }
}