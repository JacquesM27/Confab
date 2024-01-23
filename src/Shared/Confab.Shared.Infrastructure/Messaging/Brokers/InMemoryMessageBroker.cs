using Confab.Shared.Abstractions.Messaging;
using Confab.Shared.Abstractions.Modules;
using Confab.Shared.Infrastructure.Messaging.Dispatchers;

namespace Confab.Shared.Infrastructure.Messaging.Brokers;

internal sealed class InMemoryMessageBroker(
    IModuleClient moduleClient,
    IAsyncMessageDispatcher asyncMessageDispatcher,
    MessagingOptions messagingOptions
    ) : IMessageBroker
{
    
    public async Task PublishAsync(params IMessage?[]? messages)
    {
        if (messages is null)
            return;

        messages = messages.Where(x => x is not null).ToArray();
        
        if (messages.Length == 0)
            return;

        var tasks = new List<Task>();
        foreach (var message in messages)
        {
            if (messagingOptions.UseBackgroundDispatcher)
            {
                await asyncMessageDispatcher.PublishAsync(message!);
                continue;
            }
            
            tasks.Add(moduleClient.PublishAsync(message!));
        }

        await Task.WhenAll(tasks);
    }
}