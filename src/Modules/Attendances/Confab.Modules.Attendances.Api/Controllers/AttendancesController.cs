using Confab.Modules.Attendances.Application.Commands;
using Confab.Modules.Attendances.Application.DTO;
using Confab.Modules.Attendances.Application.Queries;
using Confab.Shared.Abstractions.Commands;
using Confab.Shared.Abstractions.Contexts;
using Confab.Shared.Abstractions.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Confab.Modules.Attendances.Api.Controllers;

[Authorize]
internal class AttendancesController(
    ICommandDispatcher commandDispatcher,
    IQueryDispatcher queryDispatcher,
    IContext context
    ) : BaseController
{
    [HttpGet("{conferenceId:guid}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<IReadOnlyList<AttendanceDto>>> BrowseAttendancesAsync(Guid conferenceId)
    {
        var attendances = await queryDispatcher.QueryAsync(new BrowseAttendances()
        {
            ConferenceId = conferenceId,
            UserId = context.Identity.Id
        });

        if (attendances is null)
            return NotFound();

        return Ok(attendances);
    }

    [HttpPost("events/{eventId:guid}/attend")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<ActionResult> AttendAsync(Guid eventId)
    {
        await commandDispatcher.SendAsync(new AttendEvent(eventId, context.Identity.Id));
        return NoContent();
    }
}