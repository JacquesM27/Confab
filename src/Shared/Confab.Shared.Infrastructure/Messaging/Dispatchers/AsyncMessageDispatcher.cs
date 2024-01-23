using Confab.Shared.Abstractions.Messaging;

namespace Confab.Shared.Infrastructure.Messaging.Dispatchers;

internal sealed class AsyncMessageDispatcher(IMessageChannel messageChannel) : IAsyncMessageDispatcher
{
    public async Task PublishAsync<TMessage>(TMessage message) where TMessage : class, IMessage
        => await messageChannel.Writer.WriteAsync(message);
}