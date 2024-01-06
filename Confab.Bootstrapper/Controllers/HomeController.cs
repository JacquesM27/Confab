using Microsoft.AspNetCore.Mvc;

namespace Confab.Bootstrapper.Controllers
{
    [Route("")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        public ActionResult Get()
        {
            return Ok("Confab API!");
        }
    }
}
