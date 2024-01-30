using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Agendas.Application.CallForPapers.Exceptions;

public class CallForPapersClosedException(Guid conferenceId)
    : ConfabException($"Conference with ID: '{conferenceId}' has closed call for papers.")
{
    public Guid ConferenceId => conferenceId;
}