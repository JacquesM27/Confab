using Confab.Modules.Speakers.Core.DTO;
using Confab.Modules.Speakers.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Confab.Modules.Speakers.Api.Controllers;

internal class SpeakersController(ISpeakerService speakerService) : BaseController
{
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SpeakerDto?>> Get(Guid id)
        => OkOrNotFound(await speakerService.GetAsync(id));

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<SpeakerDto>>> BrowseAsync()
        => Ok(await speakerService.BrowseAsync());

    [HttpPost]
    public async Task<ActionResult> AddAsync(SpeakerDto dto)
    {
        await speakerService.AddAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = dto.Id }, null);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateAsync(Guid id, SpeakerDto dto)
    {
        dto.Id = id;
        await speakerService.UpdateAsync(dto);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        await speakerService.DeleteAsync(id);
        return NoContent();
    }
}