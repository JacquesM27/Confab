using Confab.Modules.Agendas.Application.Submissions.Exceptions;
using Confab.Modules.Agendas.Domain.Submissions.Entities;
using Confab.Modules.Agendas.Domain.Submissions.Repositories;
using Confab.Shared.Abstractions.Commands;
using Confab.Shared.Abstractions.Kernel.Types;

namespace Confab.Modules.Agendas.Application.Submissions.Commands.Handlers;

internal sealed class CreateSubmissionHandler(
    ISubmissionRepository submissionRepository,
    ISpeakerRepository speakerRepository
    ) : ICommandHandler<CreateSubmission>
{
    public async Task HandleAsync(CreateSubmission command)
    {
        var speakerIds = command.SpeakerIds.Select(x => new AggregateId(x)).ToArray();
        var speakers = (await speakerRepository.BrowseAsync(speakerIds)).ToList();

        if (speakerIds.Length != speakers.Count)
            throw new InvalidSpeakersNumberException(command.Id);

        var submission = Submission.Create(command.Id, command.ConferenceId, command.Title, command.Description,
            command.Level, command.Tags, speakers);

        await submissionRepository.AddAsync(submission);
    }
}