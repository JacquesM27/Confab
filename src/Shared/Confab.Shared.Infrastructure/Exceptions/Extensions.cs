﻿using Confab.Shared.Abstractions.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Confab.Shared.Infrastructure.Exceptions
{
    internal static class Extensions
    {
        public static IServiceCollection AddErrorHandling(this IServiceCollection services)
        {
            services
                .AddScoped<ErrorHandlerMiddleware>()
                .AddSingleton<IExceptionsToResponseMapper, ExceptionsToResponseMapper>()
                .AddSingleton<IExceptionCompositionRoot, ExceptionCompositionRoot>();
            return services;
        }

        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
            return app;
        }
    }
}
