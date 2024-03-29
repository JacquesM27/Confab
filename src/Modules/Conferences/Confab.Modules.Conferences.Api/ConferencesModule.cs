﻿using Confab.Modules.Conferences.Core;
using Confab.Shared.Abstractions.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Confab.Modules.Conferences.Api;

internal sealed class ConferencesModule : IModule
{
    internal const string BasePath = "conferences-module";
    public string Name => "Conferences";
    public string Path => BasePath;

    public IEnumerable<string>? Policies { get; } = ["conferences", "hosts"];

    public void Register(IServiceCollection services)
    {
        services.AddCore();
    }

    public void Use(IApplicationBuilder app)
    {
    }
}