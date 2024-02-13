using Confab.Shared.Infrastructure.Api;
using Microsoft.AspNetCore.Mvc;

namespace Confab.Services.Tickets.Api.Controllers;


[Route("[controller]")]
[ApiController]
[ProducesDefaultContentType]
public abstract class BaseController : ControllerBase
{
    protected ActionResult<T> OkOrNotFound<T>(T model)
    {
        if (model is null)
            return NotFound();
        return Ok(model);
    }
}