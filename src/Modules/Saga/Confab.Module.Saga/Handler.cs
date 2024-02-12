using Chronicle;
using Confab.Modules.Saga.Messages;
using Confab.Shared.Abstractions.Events;

namespace Confab.Module.Saga;

internal class Handler(ISagaCoordinator sagaCoordinator) : IEventHandler<SpeakerCreated>, IEventHandler<SignedUp>, IEventHandler<SignedIn>
{
    public Task HandleAsync(SpeakerCreated @event) 
        => sagaCoordinator.ProcessAsync(@event, SagaContext.Empty);

    public Task HandleAsync(SignedUp @event)
        => sagaCoordinator.ProcessAsync(@event, SagaContext.Empty);

    public Task HandleAsync(SignedIn @event)
        => sagaCoordinator.ProcessAsync(@event, SagaContext.Empty);
}