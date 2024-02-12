using Confab.Modules.Speakers.Core;
using Confab.Modules.Speakers.Core.DTO;
using Confab.Modules.Speakers.Core.Services;
using Confab.Shared.Abstractions.Modules;
using Confab.Shared.Infrastructure.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Confab.Modules.Speakers.Api;

public class SpeakersModule : IModule
{
    internal const string BasePath = "speakers-module";
    public string Name => "Speakers";
    public string Path => BasePath;

    public IEnumerable<string>? Policies { get; } = ["speakers"];

    public void Register(IServiceCollection services)
    {
        services.AddCore();
    }

    public void Use(IApplicationBuilder app)
    {
        app
            .UseModuleRequest()
            .Subscribe<SpeakerDto, object>("speakers/create", async (dto, sp) =>
            {
                var service = sp.GetRequiredService<ISpeakerService>();
                await service.CreateAsync(dto);
                return null;
            });
    }
}