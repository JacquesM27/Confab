using Confab.Modules.Agendas.Application.CallForPapers.Events;
using Confab.Modules.Agendas.Application.CallForPapers.Exceptions;
using Confab.Modules.Agendas.Domain.CallForPapers.Repositories;
using Confab.Shared.Abstractions.Commands;
using Confab.Shared.Abstractions.Messaging;

namespace Confab.Modules.Agendas.Application.CallForPapers.Commands.Handlers;

internal sealed class OpenCallForPapersHandler(
    ICallForPapersRepository callForPapersRepository,
    IMessageBroker messageBroker
    ) : ICommandHandler<OpenCallForPapers>
{
    public async Task HandleAsync(OpenCallForPapers command)
    {
        var callForPapers = await callForPapersRepository.GetAsync(command.ConferenceId)
                            ?? throw new CallForPapersNotFoundException(command.ConferenceId);
        
        callForPapers.Open();
        await callForPapersRepository.UpdateAsync(callForPapers);
        await messageBroker.PublishAsync(new CallForPapersOpened(callForPapers.ConferenceId, 
            callForPapers.From, callForPapers.To));

    }
}