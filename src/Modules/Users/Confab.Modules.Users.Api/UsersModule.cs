using Confab.Modules.Users.Core;
using Confab.Shared.Abstractions.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Confab.Modules.Users.Api;

public class UsersModule : IModule
{
    internal const string BasePath = "users-module";
    public string Name => "Users";
    public string Path => BasePath;
    public void Register(IServiceCollection services)
    {
        services.AddCore();
    }

    public void Use(IApplicationBuilder app)
    {
    }
}