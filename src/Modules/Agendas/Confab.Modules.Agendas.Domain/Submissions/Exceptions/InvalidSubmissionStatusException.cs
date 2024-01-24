using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Agendas.Domain.Submissions.Exceptions;

internal sealed class InvalidSubmissionStatusException(Guid submissionId, string desiredStatus, string currentStatus) 
    : ConfabException($"Cannot change status of submission with ID: '{submissionId}' from {currentStatus} to {desiredStatus}")
{
    public Guid SubmissionId => submissionId;
}