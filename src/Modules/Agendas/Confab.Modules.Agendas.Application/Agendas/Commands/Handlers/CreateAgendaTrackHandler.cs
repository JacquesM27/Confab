using Confab.Modules.Agendas.Application.Agendas.Events;
using Confab.Modules.Agendas.Application.Agendas.Exceptions;
using Confab.Modules.Agendas.Domain.Agendas.Entities;
using Confab.Modules.Agendas.Domain.Agendas.Repositories;
using Confab.Shared.Abstractions.Commands;
using Confab.Shared.Abstractions.Messaging;

namespace Confab.Modules.Agendas.Application.Agendas.Commands.Handlers;

internal sealed class CreateAgendaTrackHandler(
    IAgendaTracksRepository repository,
    IMessageBroker messageBroker
    ) : ICommandHandler<CreateAgendaTrack>
{
    public async Task HandleAsync(CreateAgendaTrack command)
    {
        if (await repository.ExistsAsync(command.Id))
            throw new AgendaTrackAlreadyExistsException(command.Id);

        var agendaTrack = AgendaTrack.Create(command.Id, command.ConferenceId, command.Name);

        await repository.AddAsync(agendaTrack);
        await messageBroker.PublishAsync(new AgendaTrackCreated(command.Id));
    }
}