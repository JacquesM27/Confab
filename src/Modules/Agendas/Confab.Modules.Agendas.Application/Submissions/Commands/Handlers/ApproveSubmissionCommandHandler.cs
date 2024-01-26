using Confab.Modules.Agendas.Application.Submissions.Exceptions;
using Confab.Modules.Agendas.Domain.Submissions.Repositories;
using Confab.Shared.Abstractions.Commands;

namespace Confab.Modules.Agendas.Application.Submissions.Commands.Handlers;

internal sealed class ApproveSubmissionCommandHandler(
    ISubmissionRepository submissionRepository
    ) : ICommandHandler<ApproveSubmission>
{
    public async Task HandleAsync(ApproveSubmission command)
    {
        var submission = await submissionRepository.GetAsync(command.Id);

        if (submission is null)
            throw new SubmissionNotFoundException(command.Id);
        
        submission.Approve();
        await submissionRepository.UpdatedAsync(submission);
    }
}