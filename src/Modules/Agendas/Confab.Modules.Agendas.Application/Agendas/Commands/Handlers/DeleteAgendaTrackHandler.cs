using Confab.Modules.Agendas.Application.Agendas.Events;
using Confab.Modules.Agendas.Application.Agendas.Exceptions;
using Confab.Modules.Agendas.Domain.Agendas.Repositories;
using Confab.Shared.Abstractions.Commands;
using Confab.Shared.Abstractions.Messaging;

namespace Confab.Modules.Agendas.Application.Agendas.Commands.Handlers;

internal sealed class DeleteAgendaTrackHandler(
    IAgendaTracksRepository repository,
    IMessageBroker messageBroker
    ) : ICommandHandler<DeleteAgendaTrack>
{
    public async Task HandleAsync(DeleteAgendaTrack command)
    {
        var agendaTrack = await repository.GetAsync(command.Id)
                          ?? throw new AgendaTrackNotFoundException(command.Id);

        await repository.DeleteAsync(agendaTrack);
        await messageBroker.PublishAsync(new AgendaTrackDeleted(command.Id));
    }
}