using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Services.Tickets.Core.Exceptions;

public class TicketAlreadyPurchasedException(Guid conferenceId, Guid userId)
    : ConfabException($"Ticket for the conference has been already purchased.")
{
    public Guid ConferenceId => conferenceId;
    public Guid UserId => userId;
}