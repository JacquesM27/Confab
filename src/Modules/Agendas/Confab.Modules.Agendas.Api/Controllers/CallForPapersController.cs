using Confab.Modules.Agendas.Application.CallForPapers.Commands;
using Confab.Modules.Agendas.Application.CallForPapers.DTO;
using Confab.Modules.Agendas.Application.CallForPapers.Queries;
using Confab.Shared.Abstractions.Commands;
using Confab.Shared.Abstractions.Queries;
using Confab.Shared.Infrastructure.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Confab.Modules.Agendas.Api.Controllers;

[Microsoft.AspNetCore.Components.Route(AgendasModule.BasePath + "/conferences/{conferenceId:guid}/cfp")]
[Authorize(Policy)]
internal class CallForPapersController(
    ICommandDispatcher commandDispatcher,
    IQueryDispatcher queryDispatcher
    ) : BaseController
{
    private const string Policy = "cfp";

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<CallForPapersDto>> Get(Guid conferenceId)
        => OkOrNotFound(await queryDispatcher.QueryAsync(new GetCallForPapers() { ConferenceId = conferenceId }));

    [HttpPost]
    public async Task<ActionResult> CreateAsync(Guid conferenceId, CreateCallForPapers command)
    {
        await commandDispatcher.SendAsync(command.Bind(x => x.ConferenceId, conferenceId));
        return CreatedAtAction(nameof(Get), new { conferenceId = command.ConferenceId }, null);
    }

    [HttpPut("open")]
    public async Task<ActionResult> OpenAsync(Guid conferenceId)
    {
        await commandDispatcher.SendAsync(new OpenCallForPapers(conferenceId));
        return NoContent();
    }
    
    [HttpPut("close")]
    public async Task<ActionResult> CloseAsync(Guid conferenceId)
    {
        await commandDispatcher.SendAsync(new CloseCallForPapers(conferenceId));
        return NoContent();
    }
}