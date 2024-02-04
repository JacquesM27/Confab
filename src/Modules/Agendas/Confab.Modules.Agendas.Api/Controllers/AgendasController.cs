using Confab.Modules.Agendas.Application.Agendas.Commands;
using Confab.Modules.Agendas.Application.Agendas.DTO;
using Confab.Modules.Agendas.Application.Agendas.Queries;
using Confab.Shared.Abstractions.Commands;
using Confab.Shared.Abstractions.Kernel.Types;
using Confab.Shared.Abstractions.Queries;
using Confab.Shared.Infrastructure.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Confab.Modules.Agendas.Api.Controllers;

[Route(AgendasModule.BasePath + "/agendas/{conferenceId:guid}")]
[Authorize(Policy)]
internal class AgendasController(
    ICommandDispatcher commandDispatcher,
    IQueryDispatcher queryDispatcher
) : BaseController
{
    private const string Policy = "agendas";

    [HttpGet("tracks/{id:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<AgendaTrackDto>> GetAgendaTrackAsync(Guid id)
        => OkOrNotFound(await queryDispatcher.QueryAsync(new GetAgendaTrack() { Id = id }));

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<AgendaTrackDto>>> GetAgendaAsync(Guid conferenceId)
        => OkOrNotFound(await queryDispatcher.QueryAsync(new GetAgenda() { ConferenceId = conferenceId }));

    [HttpGet("items")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<AgendaItemDto>>> BrowseAgendaItemsAsync(Guid conferenceId)
        => OkOrNotFound(await queryDispatcher.QueryAsync(new BrowseAgendaItems() { ConferenceId = conferenceId }));
    
    [HttpGet("items/{id:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<AgendaItemDto>> GetAgendaItemAsync(Guid id) 
        => OkOrNotFound(await queryDispatcher.QueryAsync(new GetAgendaItem{Id = id}));

    [HttpPost("tracks")]
    public async Task<ActionResult> CreateAgendaTrackAsync(Guid conferenceId, CreateAgendaTrack command)
    {
        await commandDispatcher.SendAsync(command.Bind(x => x.ConferenceId, conferenceId));
        //await commandDispatcher.SendAsync(command);
        AddResourceIdHeader(command.Id);
        return NoContent();
    }
        
    [HttpPost("slots")]
    public async Task<ActionResult> CreateAgendaSlotAsync(CreateAgendaSlot command)
    {
        await commandDispatcher.SendAsync(command);
        AddResourceIdHeader(command.Id);
        return NoContent();
    }
        
    [HttpPut("slots/placeholder")]
    public async Task<ActionResult> AssignPlaceholderAgendaSlotAsync(AssignPlaceholderAgendaSlot command)
    {
        await commandDispatcher.SendAsync(command);
        return NoContent();
    }
        
    [HttpPut("slots/regular")]
    public async Task<ActionResult> AssignRegularAgendaSlotAsync(AssignRegularAgendaSlot command)
    {
        await commandDispatcher.SendAsync(command);
        return NoContent();
    }
    
}