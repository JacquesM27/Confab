using System.Reflection;
using Confab.Shared.Abstractions.Kernel;
using Microsoft.Extensions.DependencyInjection;

namespace Confab.Shared.Infrastructure.Kernel;

internal static class Extensions
{
    internal static IServiceCollection AddDomainEvents(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        services.AddSingleton<IDomainEventDispatcher, DomainEventDispatcher>();

        services.Scan(s => s.FromAssemblies(assemblies)
            .AddClasses(c => c.AssignableTo(typeof(IDomainEventHandler<>))
                .WithoutAttribute<DecoratorAttribute>())
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        
        return services;
    }
}