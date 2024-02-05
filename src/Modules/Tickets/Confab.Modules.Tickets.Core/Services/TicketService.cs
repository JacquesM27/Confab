using Confab.Modules.Tickets.Core.DTO;
using Confab.Modules.Tickets.Core.Entities;
using Confab.Modules.Tickets.Core.Events;
using Confab.Modules.Tickets.Core.Exceptions;
using Confab.Modules.Tickets.Core.Repositories;
using Confab.Shared.Abstractions;
using Confab.Shared.Abstractions.Messaging;
using Microsoft.Extensions.Logging;

namespace Confab.Modules.Tickets.Core.Services;

internal sealed class TicketService(
    IClock clock,
    IConferenceRepository conferenceRepository,
    ITicketRepository ticketRepository,
    ITicketSaleRepository ticketSaleRepository,
    ITicketGenerator ticketGenerator,
    ILogger<TicketService> logger,
    IMessageBroker messageBroker) : ITicketService
{
    public async Task PurchaseAsync(Guid conferenceId, Guid userId)
    {
        var conference = await conferenceRepository.GetAsync(conferenceId) 
            ?? throw new ConferenceNotFoundException(conferenceId);

        var ticket = await ticketRepository.GetAsync(conferenceId, userId);
        if (ticket is not null)
            throw new TicketAlreadyPurchasedException(conferenceId, userId);

        var ticketSale = await ticketSaleRepository.GetCurrentForConferenceAsync(conferenceId, clock.CurrentDate())
            ?? throw new TicketSaleUnavailableException(conferenceId);

        if (ticketSale.Amount.HasValue)
        {
            await PurchaseAvailableAsync(ticketSale, userId, ticketSale.Price);
            return;
        }

        ticket = ticketGenerator.Generate(conferenceId, ticketSale.Id, ticketSale.Price);
        ticket.Purchase(userId, clock.CurrentDate(), ticketSale.Price);
        await ticketRepository.AddAsync(ticket);
        logger.LogInformation($"Ticket with ID: '{ticket.Id}' was generated for the conference: " +
                              $"'{conferenceId}' by user: '{userId}'.");
        messageBroker.PublishAsync(new TicketPurchased(ticket.Id, conferenceId, userId));
    }

    private async Task PurchaseAvailableAsync(TicketSale ticketSale, Guid userId, decimal? price)
    {
        var conferenceId = ticketSale.ConferenceId;
        var ticket = ticketSale.Tickets
                         .Where(x => x.UserId is null)
                         .MinBy(_ => Guid.NewGuid())
                     ?? throw new TicketsUnavailableException(conferenceId);

        ticket.Purchase(userId, clock.CurrentDate(), price);
        await ticketRepository.UpdateAsync(ticket);
        logger.LogInformation($"Ticket with ID: '{ticket.Id}' was purchased for the conference:" +
                              $"'{conferenceId}' by user: '{userId}'");
        messageBroker.PublishAsync(new TicketPurchased(ticket.Id, conferenceId, userId));
    }
    
    public async Task<IEnumerable<TicketDto>> GetForUserAsync(Guid userId)
    {
        var tickets = await ticketRepository.GetForUserAsync(userId);

        return tickets.Select(x => new TicketDto(x.Code, x.Price, x.PurchaseAt!.Value,
                new ConferenceDto(x.ConferenceId, x.Conference.Name)))
            .OrderBy(x => x.PurchasedAt);
    }
}