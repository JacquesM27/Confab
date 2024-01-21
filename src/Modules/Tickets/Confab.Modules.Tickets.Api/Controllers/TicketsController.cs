using Confab.Modules.Tickets.Core.DTO;
using Confab.Modules.Tickets.Core.Services;
using Confab.Shared.Abstractions.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Confab.Modules.Tickets.Api.Controllers;

[Authorize]
internal sealed class TicketsController(ITicketService ticketService, IContext context) : BaseController
{
    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    public async Task<ActionResult<IReadOnlyList<TicketDto>>?> Get()
        => Ok(await ticketService.GetForUserAsync(context.Identity.Id));

    [HttpPost("conference/{conferenceId:guid}/purchase")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public async Task<ActionResult> Purchase(Guid conferenceId)
    {
        await ticketService.PurchaseAsync(conferenceId, context.Identity.Id);
        return NoContent();
    }

}