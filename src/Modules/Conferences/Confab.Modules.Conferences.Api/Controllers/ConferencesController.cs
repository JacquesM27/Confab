using Confab.Modules.Conferences.Core.DTO;
using Confab.Modules.Conferences.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Confab.Modules.Conferences.Api.Controllers;

[Authorize(Policy = Policy)]
internal class ConferencesController(IConferenceService conferenceService) : BaseController
{
    private const string Policy = "conferences";
    
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<ConferenceDetailsDto?>> Get(Guid id)
        => OkOrNotFound(await conferenceService.GetAsync(id));

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<ConferenceDto>>> BrowseAsync()
        => Ok(await conferenceService.BrowseAsync());

    [HttpPost]
    public async Task<ActionResult> AddAsync(ConferenceDetailsDto dto)
    {
        await conferenceService.AddAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = dto.Id }, null);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateAsync(Guid id, ConferenceDetailsDto dto)
    {
        dto.Id = id;
        await conferenceService.UpdateAsync(dto);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        await conferenceService.DeleteAsync(id);
        return NoContent();
    }
}