using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Agendas.Application.CallForPapers.Exceptions;

internal sealed class CallForPapersNotFoundException(Guid conferenceId) 
    : ConfabException($"Conference with ID: '{conferenceId}' has no call for papers.")
{
    public Guid ConferenceId => conferenceId;
}