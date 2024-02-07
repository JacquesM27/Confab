using Confab.Shared.Abstractions.Auth;
using Confab.Shared.Infrastructure.Auth;
using Confab.Shared.Infrastructure.Time;

namespace Confab.Shared.Tests;

public static class AuthHelper
{
    private static readonly IAuthManager AuthManager;

    static AuthHelper()
    {
        var options = OptionsHelper.GetOptions<AuthOptions>("auth");
        AuthManager = new Infrastructure.Auth.AuthManager(options, new UtcClock());
    }

    public static string CreateJwt(string userId, string? role = null, string? audience = null,
        IDictionary<string, IEnumerable<string>>? claims = null) 
        => AuthManager.CreateToken(userId, role, audience, claims).AccessToken;
}