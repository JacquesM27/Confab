using Confab.Modules.Agendas.Application.CallForPapers.Exceptions;
using Confab.Modules.Agendas.Application.Submissions.Exceptions;
using Confab.Modules.Agendas.Application.Submissions.Services;
using Confab.Modules.Agendas.Domain.CallForPapers.Repositories;
using Confab.Modules.Agendas.Domain.Submissions.Entities;
using Confab.Modules.Agendas.Domain.Submissions.Repositories;
using Confab.Shared.Abstractions.Commands;
using Confab.Shared.Abstractions.Kernel;
using Confab.Shared.Abstractions.Kernel.Types;
using Confab.Shared.Abstractions.Messaging;

namespace Confab.Modules.Agendas.Application.Submissions.Commands.Handlers;

internal sealed class CreateSubmissionHandler(
    ISubmissionRepository submissionRepository,
    ISpeakerRepository speakerRepository,
    IEventMapper eventMapper,
    IMessageBroker messageBroker,
    IDomainEventDispatcher domainEventDispatcher,
    ICallForPapersRepository callForPapersRepository
    ) : ICommandHandler<CreateSubmission>
{
    public async Task HandleAsync(CreateSubmission command)
    {
        var callForPapers = await callForPapersRepository.GetAsync(command.ConferenceId);
        if (callForPapers is null)
            throw new CallForPapersNotFoundException(command.ConferenceId);

        if (callForPapers.IsOpened is false)
            throw new CallForPapersClosedException(command.ConferenceId);
        
        var speakerIds = command.SpeakerIds.Select(x => new AggregateId(x)).ToArray();
        var test = await speakerRepository.BrowseAsync(speakerIds);
        var speakers = test.ToList();

        if (speakerIds.Length != speakers.Count)
            throw new InvalidSpeakersNumberException(command.Id);

        var submission = Submission.Create(command.Id, command.ConferenceId, command.Title, command.Description,
            command.Level, command.Tags, speakers);

        var events = eventMapper.MapAll(submission.Events);
        
        await submissionRepository.AddAsync(submission);
        await domainEventDispatcher.DispatchAsync(submission.Events.ToArray());
        await messageBroker.PublishAsync(events.ToArray());
    }
}