using Confab.Shared.Abstractions.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace Confab.Shared.Infrastructure.Exceptions
{
    internal class ExceptionCompositionRoot(IServiceProvider serviceProvider) : IExceptionCompositionRoot
    {
        public ExceptionResponse Map(Exception exception)
        {
            using var scope = serviceProvider.CreateScope();
            var mappers = scope.ServiceProvider.GetServices<IExceptionsToResponseMapper>().ToArray();
            var nonDefaultMappers = mappers.Where(x => x is not ExceptionsToResponseMapper);
            var result = nonDefaultMappers
                .Select(x => x.Map(exception))
                .SingleOrDefault(x => x is not null);

            if (result is not null)
                return result;

            var defaultMapper = mappers.SingleOrDefault(x => x is ExceptionsToResponseMapper);
            return defaultMapper!.Map(exception)!;
        }
    }
}
