﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Confab.Shared.Abstractions.Modules;

public interface IModule
{
    public string Name { get; }
    public string Path { get; }
    IEnumerable<string>? Policies => null;

    void Register(IServiceCollection services);
    void Use(IApplicationBuilder app);
}