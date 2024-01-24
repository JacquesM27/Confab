using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Agendas.Domain.Submissions.Exceptions;

internal sealed class InvalidSubmissionLevelException(Guid submissionId) 
    : ConfabException($"Submission with ID: '{submissionId}' defines invalid level.")
{
    public Guid SubmissionId => submissionId;
}