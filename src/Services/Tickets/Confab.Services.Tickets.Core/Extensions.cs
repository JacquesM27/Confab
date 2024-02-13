using System.Runtime.CompilerServices;
using Confab.Services.Tickets.Core.DAL;
using Confab.Services.Tickets.Core.DAL.Repositories;
using Confab.Services.Tickets.Core.Repositories;
using Confab.Services.Tickets.Core.Services;
using Confab.Shared.Abstractions;
using Confab.Shared.Abstractions.Contexts;
using Confab.Shared.Abstractions.Storage;
using Confab.Shared.Infrastructure.Api;
using Confab.Shared.Infrastructure.Auth;
using Confab.Shared.Infrastructure.Commands;
using Confab.Shared.Infrastructure.Contexts;
using Confab.Shared.Infrastructure.Events;
using Confab.Shared.Infrastructure.Exceptions;
using Confab.Shared.Infrastructure.Kernel;
using Confab.Shared.Infrastructure.Messaging;
using Confab.Shared.Infrastructure.Modules;
using Confab.Shared.Infrastructure.Postgres;
using Confab.Shared.Infrastructure.Queries;
using Confab.Shared.Infrastructure.Services;
using Confab.Shared.Infrastructure.Storage;
using Confab.Shared.Infrastructure.Time;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("Confab.Services.Tickets.Api")]
namespace Confab.Services.Tickets.Core;

internal static class Extensions
{
    internal static IServiceCollection AddCore(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        services.AddAuth();
        services.AddMemoryCache();
        services.AddSingleton<IRequestStorage, RequestStorage>();
        services.AddSingleton<IContextFactory, ContextFactory>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddTransient<IContext>(sp => sp.GetRequiredService<IContextFactory>().Create());

        services.AddModuleRequest(assemblies);
        
        services.AddErrorHandling();

        services.AddCommands(assemblies);
        services.AddQueries(assemblies);
        services.AddDomainEvents(assemblies);
        services.AddEvents(assemblies);
        services.AddMessaging();

        services.AddPostgres();
        services.AddTransactionalDecorations();
        
        services.AddSingleton<IClock, UtcClock>();
        services.AddHostedService<AppInitializer>();
        services.AddControllers();
        
        
        services.AddPostgres<TicketsDbContext>();
        services.AddScoped<IConferenceRepository, ConferenceRepository>();
        services.AddScoped<ITicketService, TicketService>();
        services.AddScoped<ITicketSaleService, TicketSaleService>();
        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddScoped<ITicketSaleRepository, TicketSaleRepository>();
        services.AddSingleton<ITicketGenerator, TicketGenerator>();
        return services;
    }
}