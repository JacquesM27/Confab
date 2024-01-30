using Confab.Modules.Agendas.Application.CallForPapers.Events;
using Confab.Modules.Agendas.Application.CallForPapers.Exceptions;
using Confab.Modules.Agendas.Domain.CallForPapers.Repositories;
using Confab.Shared.Abstractions.Commands;
using Confab.Shared.Abstractions.Messaging;

namespace Confab.Modules.Agendas.Application.CallForPapers.Commands.Handlers;

internal sealed class CreateCallForPapersHandler(
    ICallForPapersRepository callForPapersRepository,
    IMessageBroker messageBroker
    ) : ICommandHandler<CreateCallForPapers>
{
    public async Task HandleAsync(CreateCallForPapers command)
    {
        if (await callForPapersRepository.ExistsAsync(command.ConferenceId))
            throw new CallForPapersAlreadyExistsException(command.ConferenceId);

        var callForPapers =
            Domain.CallForPapers.Entities.CallForPapers.Create(command.Id, command.ConferenceId, command.From,
                command.To);

        await callForPapersRepository.AddAsync(callForPapers);
        await messageBroker.PublishAsync(new CallForPapersCreated(callForPapers.Id, callForPapers.ConferenceId));
    }
}