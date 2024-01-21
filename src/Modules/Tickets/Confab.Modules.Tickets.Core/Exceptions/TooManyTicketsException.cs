using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Tickets.Core.Exceptions;

internal sealed class TooManyTicketsException(Guid conferenceId) 
    : ConfabException($"Too many tickets would be generated for the conference.")
{
    public Guid ConferenceId => conferenceId;
}