using Confab.Modules.Agendas.Domain.Submissions.Entities;
using Confab.Modules.Agendas.Domain.Submissions.Repositories;
using Confab.Shared.Abstractions.Events;

namespace Confab.Modules.Agendas.Application.Submissions.Events.External.Handlers;

internal sealed class SpeakerCreatedHandler(ISpeakerRepository speakerRepository) : IEventHandler<SpeakerCreated>
{
    public async Task HandleAsync(SpeakerCreated @event)
    {
        if (await speakerRepository.ExistsAsync(@event.Id))
            return;

        var speaker = new Speaker(@event.Id, @event.FullName);
        await speakerRepository.AddAsync(speaker);
    }
}