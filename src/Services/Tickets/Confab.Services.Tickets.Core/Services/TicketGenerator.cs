using Confab.Services.Tickets.Core.Entities;
using Confab.Shared.Abstractions;

namespace Confab.Services.Tickets.Core.Services;

internal sealed class TicketGenerator(IClock clock) : ITicketGenerator
{
    public Ticket Generate(Guid conferenceId, Guid ticketSaleId, decimal? price)
        => new()
        {
            Id = Guid.NewGuid(),
            TicketSaleId = ticketSaleId,
            ConferenceId = conferenceId,
            Code = Guid.NewGuid().ToString("N"),
            Price = price,
            CreatedAt = clock.CurrentDate()
        };
}