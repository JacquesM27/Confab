using Microsoft.AspNetCore.Mvc;

namespace Confab.Modules.Tickets.Api.Controllers;

[Route(TicketsModule.BasePath)]
internal sealed class HomeController : BaseController
{
    [HttpGet]
    public ActionResult<string> Get() => "Tickets API";
}