﻿using Confab.Modules.Conferences.Core.DTO;
using Confab.Modules.Conferences.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Confab.Modules.Conferences.Api.Controllers;

internal class HostsController(IHostService hostService) : BaseController
{
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<HostDetailsDto?>> Get(Guid id)
        => OkOrNotFound(await hostService.GetAsync(id));

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<HostDto>>> BrowseAsync()
        => Ok(await hostService.BrowseAsync());

    [HttpPost]
    public async Task<ActionResult> AddAsync(HostDto dto)
    {
        await hostService.AddAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = dto.Id }, null);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateAsync(Guid id, HostDetailsDto dto)
    {
        dto.Id = id;
        await hostService.UpdateAsync(dto);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        await hostService.DeleteAsync(id);
        return NoContent();
    }
}