using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Agendas.Domain.Submissions.Exceptions;

internal sealed class MissingSubmissionSpeakersException(Guid submissionId) 
    : ConfabException($"Submission with ID: '{submissionId}' has missing speakers.")
{
    public Guid SubmissionId => submissionId;
}