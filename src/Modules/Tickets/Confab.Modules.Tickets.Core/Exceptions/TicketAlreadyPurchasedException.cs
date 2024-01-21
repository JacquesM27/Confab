using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Tickets.Core.Exceptions;

public class TicketAlreadyPurchasedException(Guid conferenceId, Guid userId)
    : ConfabException($"Ticket for the conference has been already purchased.")
{
    public Guid ConferenceId => conferenceId;
    public Guid UserId => userId;
}