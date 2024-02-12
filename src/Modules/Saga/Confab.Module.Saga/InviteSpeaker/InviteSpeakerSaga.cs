using Chronicle;
using Confab.Modules.Saga.Messages;
using Confab.Shared.Abstractions.Messaging;
using Confab.Shared.Abstractions.Modules;

namespace Confab.Module.Saga.InviteSpeaker;

internal sealed class InviteSpeakerSaga(
    IModuleClient moduleClient,
    IMessageBroker messageBroker
) : Chronicle.Saga<InviteSpeakerSaga.SagaData>, 
    ISagaStartAction<SignedUp>, 
    ISagaAction<SpeakerCreated>,
    ISagaAction<SignedIn>
{
    public override SagaId ResolveId(object message, ISagaContext context)
        => message switch
        {
            SignedUp m => m.UserId.ToString(),
            SignedIn m => m.UserId.ToString(),
            SpeakerCreated m => m.Id.ToString(),
            _ => base.ResolveId(message, context)
        };

    public async Task HandleAsync(SignedUp message, ISagaContext context)
    {
        var (userId, email) = message;

        if (Data.InvitedSpeakers.TryGetValue(email, out var fullName))
        {
            Data.Email = email;
            Data.FullName = fullName;
            await moduleClient.SendAsync("speakers/create", new
            {
                Id = userId,
                Email = email,
                FullName = fullName,
                Bio = "Lorem Ipsum"
            });
            return;
        }

        await CompleteAsync();
    }

    public Task HandleAsync(SpeakerCreated message, ISagaContext context)
    {
        Data.SpeakerCreated = true;
        return Task.CompletedTask;
    }

    public async Task HandleAsync(SignedIn message, ISagaContext context)
    {
        if (Data.SpeakerCreated)
        {
            await messageBroker.PublishAsync(new SendWelcomeMessage(Data.Email, Data.FullName));
            await CompleteAsync();
        }
    }
    
    public Task CompensateAsync(SpeakerCreated message, ISagaContext context)
        => Task.CompletedTask;
    
    public Task CompensateAsync(SignedUp message, ISagaContext context)
        => Task.CompletedTask;

    public Task CompensateAsync(SignedIn message, ISagaContext context)
        => Task.CompletedTask;

    internal class SagaData
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public bool SpeakerCreated { get; set; }

        public readonly Dictionary<string, string> InvitedSpeakers = new()
        {
            ["test1speaker@confab.io"] = "John Smith",
            ["test2speaker@confab.io"] = "Mark Sim"
        };
    }
}