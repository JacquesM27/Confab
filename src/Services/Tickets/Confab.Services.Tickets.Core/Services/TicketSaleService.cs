using Confab.Services.Tickets.Core.DTO;
using Confab.Services.Tickets.Core.Entities;
using Confab.Services.Tickets.Core.Exceptions;
using Confab.Services.Tickets.Core.Repositories;
using Confab.Shared.Abstractions;
using Microsoft.Extensions.Logging;

namespace Confab.Services.Tickets.Core.Services;

internal sealed class TicketSaleService(
    IConferenceRepository conferenceRepository,
    ITicketSaleRepository ticketSaleRepository,
    ITicketRepository ticketRepository,
    ITicketGenerator ticketGenerator,
    IClock clock,
    ILogger<TicketSaleService> logger) 
    : ITicketSaleService
{
    public async Task AddAsync(TicketSaleDto dto)
    {
        var conference = await conferenceRepository.GetAsync(dto.ConferenceId) ??
                         throw new ConferenceNotFoundException(dto.ConferenceId);

        if (conference.ParticipantsLimit.HasValue)
        {
            var ticketsCount = await ticketRepository.CountForConferenceAsync(conference.Id);
            if (ticketsCount + dto.Amount > conference.ParticipantsLimit)
                throw new TooManyTicketsException(conference.Id);
        }
        
        dto.Id = Guid.NewGuid();
        TicketSale ticketSale = new()
        {
            Id = dto.Id,
            ConferenceId = dto.ConferenceId,
            From = dto.From,
            To = dto.To,
            Amount = dto.Amount,
            Price = dto.Price,
            Name = dto.Name
        };
        await ticketSaleRepository.AddAsync(ticketSale);
        logger.LogInformation($"Added a ticket sale conference with ID: '{conference.Id}' ({dto.From} - {dto.To}).");

        if (ticketSale.Amount.HasValue)
        {
            logger.LogInformation($"Generating {ticketSale.Amount} tickets for conference with ID: '{conference.Id}'...");
            var tickets = new List<Ticket>();
            for (var i = 0; i < ticketSale.Amount; i++)
            {
                var ticket = ticketGenerator.Generate(conference.Id, ticketSale.Id, ticketSale.Price);
                tickets.Add(ticket);
            }

            await ticketRepository.AddManyAsync(tickets);
        }
    }

    public async Task<IEnumerable<TicketSaleInfoDto>?> GetAllAsync(Guid conferenceId)
    {
        var conference = await conferenceRepository.GetAsync(conferenceId);
        if (conference is null)
            return null;

        var ticketSales = await ticketSaleRepository.BrowseForConferenceAsync(conferenceId);

        return ticketSales.Select(x => Map(x, conference));
    }

    public async Task<TicketSaleInfoDto?> GetCurrentAsync(Guid conferenceId)
    {
        var conference = await conferenceRepository.GetAsync(conferenceId);
        if (conference is null)
            return null;

        var now = clock.CurrentDate();
        var ticketSale = await ticketSaleRepository.GetCurrentForConferenceAsync(conferenceId, now);

        return ticketSale is null ? null : Map(ticketSale, conference);
    }

    private static TicketSaleInfoDto Map(TicketSale ticketSale, Conference conference)
    {
        int? availableTickets = null;
        var totalTickets = ticketSale.Amount;
        if (totalTickets.HasValue)
            availableTickets = ticketSale.Tickets.Count(x => x.UserId is null);

        return new TicketSaleInfoDto(ticketSale.Name, new ConferenceDto(conference.Id, conference.Name),
            ticketSale.Price, totalTickets, availableTickets, ticketSale.From, ticketSale.To);
    }
}