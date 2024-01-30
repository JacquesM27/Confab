using Confab.Modules.Agendas.Application.Submissions.Exceptions;
using Confab.Modules.Agendas.Application.Submissions.Services;
using Confab.Modules.Agendas.Domain.Submissions.Repositories;
using Confab.Shared.Abstractions.Commands;
using Confab.Shared.Abstractions.Kernel;
using Confab.Shared.Abstractions.Messaging;

namespace Confab.Modules.Agendas.Application.Submissions.Commands.Handlers;

internal sealed class RejectCommandHandler(
    ISubmissionRepository submissionRepository,
    IDomainEventDispatcher domainEventDispatcher,
    IEventMapper eventMapper,
    IMessageBroker messageBroker
    ) : ICommandHandler<RejectSubmission>
{
    public async Task HandleAsync(RejectSubmission command)
    {
        var submission = await submissionRepository.GetAsync(command.Id);

        if (submission is null)
            throw new SubmissionNotFoundException(command.Id);
            
        submission.Rejected();
        
        var events = eventMapper.MapAll(submission.Events);
        
        await submissionRepository.UpdatedAsync(submission);
        await domainEventDispatcher.DispatchAsync(submission.Events.ToArray());
        await messageBroker.PublishAsync(events.ToArray());
    }
}
