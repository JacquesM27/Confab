using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Agendas.Domain.Submissions.Exceptions;

internal sealed class EmptySubmissionDescriptionException(Guid submissionId) 
    : ConfabException($"Submission with ID: '{submissionId}' defines empty description.")
{
    public Guid SubmissionId => submissionId;
}