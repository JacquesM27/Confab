using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Agendas.Application.CallForPapers.Exceptions;

internal sealed class CallForPapersAlreadyExistsException(Guid conferenceId) 
    : ConfabException($"Conference with ID: '{conferenceId}' already defined call for papers.")
{
    public Guid ConferenceId => conferenceId;
}