using Confab.Modules.Agendas.Application.CallForPapers.Events;
using Confab.Modules.Agendas.Application.CallForPapers.Exceptions;
using Confab.Modules.Agendas.Domain.CallForPapers.Repositories;
using Confab.Shared.Abstractions.Commands;
using Confab.Shared.Abstractions.Messaging;

namespace Confab.Modules.Agendas.Application.CallForPapers.Commands.Handlers;

internal sealed class CloseCallForPapersHandler(
    ICallForPapersRepository callForPapersRepository,
    IMessageBroker messageBroker
    ) : ICommandHandler<CloseCallForPapers>
{
    public async Task HandleAsync(CloseCallForPapers command)
    {
        var callForPapers = await callForPapersRepository.GetAsync(command.ConferenceId)
                            ?? throw new CallForPapersNotFoundException(command.ConferenceId);
        
        callForPapers.Close();
        await callForPapersRepository.UpdateAsync(callForPapers);
        await messageBroker.PublishAsync(new CallForPapersClosed(callForPapers.ConferenceId));
    }
}