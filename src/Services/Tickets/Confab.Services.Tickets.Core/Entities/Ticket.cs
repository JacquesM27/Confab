using Confab.Services.Tickets.Core.Exceptions;

namespace Confab.Services.Tickets.Core.Entities;

public class Ticket
{

    public Guid Id { get; set; }
    public Guid TicketSaleId { get; set; }
    public TicketSale TicketSale { get; set; }
    public Guid ConferenceId { get; set; }
    public Conference Conference { get; set; }
    public decimal? Price { get; set; }
    public string Code { get; set; } = string.Empty;
    public Guid? UserId { get; set; }
    public DateTime? PurchaseAt { get; set; }
    public DateTime? UsedAt { get; set; }
    public DateTime CreatedAt { get; set; }

    public void Purchase(Guid userId, DateTime purchasedAt, decimal? price)
    {
        if (UserId.HasValue)
            throw new TicketAlreadyPurchasedException(ConferenceId, UserId.Value);

        UserId = userId;
        PurchaseAt = purchasedAt;
        Price = price;
    }
}