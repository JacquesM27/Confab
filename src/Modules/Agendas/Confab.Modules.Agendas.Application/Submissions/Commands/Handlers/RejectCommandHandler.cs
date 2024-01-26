using Confab.Modules.Agendas.Application.Submissions.Exceptions;
using Confab.Modules.Agendas.Domain.Submissions.Repositories;
using Confab.Shared.Abstractions.Commands;

namespace Confab.Modules.Agendas.Application.Submissions.Commands.Handlers;

internal sealed class RejectCommandHandler(
        ISubmissionRepository submissionRepository
    ) : ICommandHandler<RejectSubmission>
{
    public async Task HandleAsync(RejectSubmission command)
    {
        var submission = await submissionRepository.GetAsync(command.Id);

        if (submission is null)
            throw new SubmissionNotFoundException(command.Id);
            
        submission.Rejected();
        await submissionRepository.UpdatedAsync(submission);
    }
}
