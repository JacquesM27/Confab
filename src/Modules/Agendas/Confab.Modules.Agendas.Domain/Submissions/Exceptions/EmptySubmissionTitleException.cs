using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Agendas.Domain.Submissions.Exceptions;

internal sealed class EmptySubmissionTitleException(Guid submissionId) 
    : ConfabException($"Submission with ID: '{submissionId}' defines empty title.")
{
    public Guid SubmissionId => submissionId;
}