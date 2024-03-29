﻿using Confab.Shared.Infrastructure.Api;
using Microsoft.AspNetCore.Mvc;

namespace Confab.Modules.Agendas.Api.Controllers;

[Route(AgendasModule.BasePath + "/[controller]")]
[ApiController]
[ProducesDefaultContentType]
internal abstract class BaseController : ControllerBase
{
    protected ActionResult<T> OkOrNotFound<T>(T model)
    {
        if (model is null)
            return NotFound();
        return Ok(model);
    }
    
    protected void AddResourceIdHeader(Guid id) => Response.Headers.Add("Resource-ID", id.ToString());
}