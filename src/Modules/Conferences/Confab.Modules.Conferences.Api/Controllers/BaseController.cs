using Microsoft.AspNetCore.Mvc;

namespace Confab.Modules.Conferences.Api.Controllers;

[Route(ConferencesModule.BasePath + "/[controller]")]
[ApiController]
internal abstract class BaseController : ControllerBase
{

    protected ActionResult<T> OkOrNotFound<T>(T model)
    {
        if (model is null)
            return NotFound();
        return Ok(model);
    }
}