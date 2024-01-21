﻿using System.Runtime.CompilerServices;
using Confab.Modules.Tickets.Core.DAL;
using Confab.Modules.Tickets.Core.DAL.Repositories;
using Confab.Modules.Tickets.Core.Repositories;
using Confab.Modules.Tickets.Core.Services;
using Confab.Shared.Infrastructure.Postgres;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("Confab.Modules.Tickets.Api")]
namespace Confab.Modules.Tickets.Core;

internal static class Extensions
{
    internal static IServiceCollection AddCore(this IServiceCollection services)
    {
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