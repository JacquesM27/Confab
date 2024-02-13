using Confab.Services.Tickets.Core.DTO;
using Confab.Services.Tickets.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Confab.Services.Tickets.Api.Controllers;

public sealed class SalesController(ITicketSaleService ticketSaleService) : BaseController
{
    private const string Policy = "tickets";

    [HttpGet("conferences/{conferenceId:guid}")]
    [ProducesResponseType(200)]
    public async Task<ActionResult<IEnumerable<TicketSaleInfoDto>?>> GetAll(Guid conferenceId)
        => OkOrNotFound(await ticketSaleService.GetAllAsync(conferenceId));

    [HttpGet("conferences/{conferenceId:guid}/current")]
    [ProducesResponseType(200)]
    public async Task<ActionResult<TicketSaleInfoDto?>> GetCurrent(Guid conferenceId)
        => OkOrNotFound(await ticketSaleService.GetCurrentAsync(conferenceId));

    [Authorize(Policy)]
    [HttpPost("conferences/{conferenceId:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    public async Task<ActionResult> Post(Guid conferenceId, TicketSaleDto dto)
    {
        dto.ConferenceId = conferenceId;
        await ticketSaleService.AddAsync(dto);
        return NoContent();
    }
}