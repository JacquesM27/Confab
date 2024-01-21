using Confab.Shared.Infrastructure.Api;
using Microsoft.AspNetCore.Mvc;

namespace Confab.Modules.Tickets.Api.Controllers;


[Route(TicketsModule.BasePath + "/[controller]")]
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
}