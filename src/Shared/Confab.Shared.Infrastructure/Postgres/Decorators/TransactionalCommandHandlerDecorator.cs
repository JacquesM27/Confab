using Confab.Shared.Abstractions.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Confab.Shared.Infrastructure.Postgres.Decorators;

[Decorator]
internal class TransactionalCommandHandlerDecorator<T>(
    ICommandHandler<T> handler, 
    UnitOfWorkTypeRegistry unitOfWorkTypeRegistry, 
    IServiceProvider serviceProvider
    ) : ICommandHandler<T> where T : class, ICommand
{
    public async Task HandleAsync(T command)
    {
        var unitOfWorkType = unitOfWorkTypeRegistry.Resolve<T>();
        if (unitOfWorkType is null)
        {
            await handler.HandleAsync(command);
            return;
        }

        var unitOfWork = (IUnitOfWork)serviceProvider.GetRequiredService(unitOfWorkType);
        await unitOfWork.ExecuteAsync(() => handler.HandleAsync(command));
    }
}