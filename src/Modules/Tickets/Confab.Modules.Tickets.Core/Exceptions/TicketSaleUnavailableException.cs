using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Tickets.Core.Exceptions;

internal sealed class TicketSaleUnavailableException(Guid conferenceId) 
    : ConfabException("Ticket sale for the conference is unavailable.")
{
    public Guid ConferenceId => conferenceId;
}