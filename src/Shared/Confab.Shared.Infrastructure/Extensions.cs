﻿using System.Reflection;
using System.Runtime.CompilerServices;
using Confab.Shared.Abstractions;
using Confab.Shared.Abstractions.Contexts;
using Confab.Shared.Abstractions.Modules;
using Confab.Shared.Infrastructure.Api;
using Confab.Shared.Infrastructure.Auth;
using Confab.Shared.Infrastructure.Contexts;
using Confab.Shared.Infrastructure.Events;
using Confab.Shared.Infrastructure.Exceptions;
using Confab.Shared.Infrastructure.Modules;
using Confab.Shared.Infrastructure.Services;
using Confab.Shared.Infrastructure.Time;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

[assembly:InternalsVisibleTo("Confab.Bootstrapper")]
namespace Confab.Shared.Infrastructure;

internal static class Extensions
{
    private const string CorsPolicy = "cors";
    
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IList<Assembly> assemblies, IList<IModule> modules)
    {
        var disabledModules = new List<string>();
        using (var serviceProvider = services.BuildServiceProvider())
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            foreach (var keyValuePair in configuration.AsEnumerable())
            {
                if (!keyValuePair.Key.Contains(":module:enabled"))
                    continue;
                
                if (!bool.Parse(keyValuePair.Value!))
                    disabledModules.Add(keyValuePair.Key!.Split(":")[0]);
            }
        }
        
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        services.AddCors(cors =>
        {
            cors.AddPolicy(CorsPolicy, x =>
            {
                x.WithOrigins("*")
                    .WithMethods("POST", "PUT", "DELETE")
                    .WithHeaders("Content-Type", "Authorization");
            });
        });

        services.AddSwaggerGen(swagger =>
        {
            swagger.CustomSchemaIds(x => x.FullName);
            swagger.SwaggerDoc("v1", new OpenApiInfo()
            {
                Title = "Confab API",
                Version = "v1"
            });
        });

        services.AddSingleton<IContextFactory, ContextFactory>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddTransient<IContext>(sp => sp.GetRequiredService<IContextFactory>().Create());
        
        services.AddModuleInfo(modules);

        services.AddModuleRequest(assemblies);
        
        services.AddAuth(modules);
        services.AddErrorHandling();

        services.AddEvents(assemblies);
        
        services.AddSingleton<IClock, UtcClock>();
        services.AddHostedService<AppInitializer>();
        services
            .AddControllers()
            .ConfigureApplicationPartManager(manager =>
            {
                var removedParts = new List<ApplicationPart>();
                foreach (var disabledModule in disabledModules)
                {
                    var parts = manager.ApplicationParts.Where(x => x.Name.Contains(disabledModule,
                        StringComparison.InvariantCultureIgnoreCase));
                    removedParts.AddRange(parts);
                }
                
                foreach (var part in removedParts)
                {
                    manager.ApplicationParts.Remove(part);
                }
                
                manager.FeatureProviders.Add(new InternalControllerFeatureProvider());
            });

        return services;
    }

    public static WebApplication UseInfrastructure(this WebApplication app)
    {
        app.UseCors(CorsPolicy);
        
        app.UseErrorHandling();

        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseReDoc(reDoc =>
        {
            reDoc.RoutePrefix = "docs";
            reDoc.SpecUrl("/swagger/v1/swagger.json");
            reDoc.DocumentTitle = "Confab API";
        });

        app.UseHttpsRedirection();

        app.UseAuthentication();
        
        app.UseAuthorization();

        return app;
    }

    public static T GetOptions<T>(this IServiceCollection services, string sectionName)
        where T : new()
    {
        using var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        return configuration.GetOptions<T>(sectionName);
    }

    public static T GetOptions<T>(this IConfiguration configuration, string sectionName)
        where T : new()
    {
        var options = new T();
        configuration.GetSection(sectionName).Bind(options);
        return options;
    }
}