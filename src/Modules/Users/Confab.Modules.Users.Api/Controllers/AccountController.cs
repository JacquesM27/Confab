using Confab.Modules.Users.Core.DTO;
using Confab.Modules.Users.Core.Services;
using Confab.Shared.Abstractions.Auth;
using Confab.Shared.Abstractions.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Confab.Modules.Users.Api.Controllers;

internal class AccountController(IIdentityService identityService, IContext context) : BaseController
{
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<AccountDto?>> GetAsync()
        => OkOrNotFound(await identityService.GetAsync(context.Identity.Id));

    [HttpPost("sign-up")]
    public async Task<ActionResult> SignUpAsync(SignUpDto dto)
    {
        await identityService.SignUpAsync(dto);
        return NoContent();
    }

    [HttpPost("sign-in")]
    public async Task<ActionResult<JsonWebToken>> SignInAsync(SignInDto dto)
        => Ok(await identityService.SignInAsync(dto));
}