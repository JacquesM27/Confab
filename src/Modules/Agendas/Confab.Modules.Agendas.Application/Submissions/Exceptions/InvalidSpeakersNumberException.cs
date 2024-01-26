using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Agendas.Application.Submissions.Exceptions;

internal sealed class InvalidSpeakersNumberException(Guid submissionId) 
    : ConfabException($"Submission with ID: '{submissionId}' has invalid member speakers.")
{
    public Guid SubmissionId => submissionId;
}