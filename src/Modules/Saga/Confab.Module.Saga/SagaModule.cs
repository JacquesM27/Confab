using Confab.Shared.Abstractions.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Confab.Module.Saga;

public class SagaModule : IModule
{
    private const string BasePath = "saga-module";
    public string Name { get; } = "Saga";
    public string Path => BasePath;
    public void Register(IServiceCollection services)
    {
        services.AddSaga(); 
    }

    public void Use(IApplicationBuilder app)
    {
        
    }
}