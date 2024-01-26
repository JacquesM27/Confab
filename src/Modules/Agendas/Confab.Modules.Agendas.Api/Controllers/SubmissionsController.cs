using Confab.Shared.Abstractions.Commands;
using Confab.Modules.Agendas.Application.Submissions.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Confab.Modules.Agendas.Api.Controllers;

internal class SubmissionsController(ICommandDispatcher commandDispatcher) : BaseController
{
    [HttpPost]
    public async Task<ActionResult> CreateAsync(CreateSubmission command)
    {
        await commandDispatcher.SendAsync(command);
        return CreatedAtAction("Get", new { id = command.Id }, null);
    }
    
    [HttpPut("{id:guid}/approve")]
    public async Task<ActionResult> ApproveAsync(Guid id)
    {
        await commandDispatcher.SendAsync(new ApproveSubmission(id));
        return NoContent();
    }
    
    [HttpPut("{id:guid}/reject")]
    public async Task<ActionResult> RejectAsync(Guid id)
    {
        await commandDispatcher.SendAsync(new RejectSubmission(id));
        return NoContent();
    }
}