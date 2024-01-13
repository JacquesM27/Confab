using System.Runtime.CompilerServices;
using Confab.Modules.Speakers.Core.Repositories;
using Confab.Modules.Speakers.Core.Services;
using Microsoft.Extensions.DependencyInjection;

[assembly:InternalsVisibleTo("Confab.Modules.Speakers.Api")]
namespace Confab.Modules.Speakers.Core;

internal static class Extensions
{
    internal static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddSingleton<ISpeakerRepository, InMemorySpeakerRepository>();
        services.AddScoped<ISpeakerService, SpeakerService>();

        return services;
    }
}