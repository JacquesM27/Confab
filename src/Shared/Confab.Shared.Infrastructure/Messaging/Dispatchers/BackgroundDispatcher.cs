using Confab.Shared.Abstractions.Modules;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Confab.Shared.Infrastructure.Messaging.Dispatchers;

internal sealed class BackgroundDispatcher(
    IMessageChannel messageChannel,
    IModuleClient moduleClient,
    ILogger<BackgroundDispatcher> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation($"Running the background dispatcher...");

        await foreach (var message in messageChannel.Reader.ReadAllAsync(stoppingToken))
        {
            try
            {
                await moduleClient.PublishAsync(message);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, exception.Message);
            }
        }
        
        logger.LogInformation("Finished running the background dispatcher.");
    }
}