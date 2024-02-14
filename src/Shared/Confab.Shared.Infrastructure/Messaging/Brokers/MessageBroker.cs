using Confab.Shared.Abstractions.Messaging;
using Confab.Shared.Abstractions.Modules;
using Confab.Shared.Infrastructure.Messaging.Dispatchers;
using Convey.MessageBrokers;

namespace Confab.Shared.Infrastructure.Messaging.Brokers;

internal sealed class MessageBroker(
    IModuleClient moduleClient,
    IAsyncMessageDispatcher asyncMessageDispatcher,
    MessagingOptions messagingOptions,
    IBusPublisher busPublisher
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
            await busPublisher.PublishAsync(message);  
            
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