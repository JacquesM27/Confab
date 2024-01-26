using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Agendas.Application.Submissions.Exceptions;

internal sealed class SubmissionNotFoundException(Guid submissionId) 
    : ConfabException($"Submission with ID: '{submissionId}' was not found.")
{
    public Guid SubmissionId => submissionId;
}