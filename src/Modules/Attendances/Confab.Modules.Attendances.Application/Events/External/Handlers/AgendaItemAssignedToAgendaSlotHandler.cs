using Confab.Modules.Attendances.Application.Clients.Agendas;
using Confab.Modules.Attendances.Domain.Entities;
using Confab.Modules.Attendances.Domain.Exceptions;
using Confab.Modules.Attendances.Domain.Policies;
using Confab.Modules.Attendances.Domain.Repositories;
using Confab.Shared.Abstractions.Events;

namespace Confab.Modules.Attendances.Application.Events.External.Handlers;

internal sealed class AgendaItemAssignedToAgendaSlotHandler(
    IAttendableEventRepository attendableEventRepository,
    ISlotPolicyFactory slotPolicyFactory,
    IAgendasApiClient agendasApiClient
    ) : IEventHandler<AgendaItemAssignedToAgendaSlot>
{
    public async Task HandleAsync(AgendaItemAssignedToAgendaSlot @event)
    {
        var attendableEvent = await attendableEventRepository.GetAsync(@event.AgendaItemId);
        if (attendableEvent is not null)
            return;

        var slot = await agendasApiClient.GetRegularAgendaSlotAsync(@event.AgendaItemId)
            ?? throw new AttendableEventNotFoundException(@event.AgendaItemId);
        
        if (!slot.ParticipantsLimit.HasValue)
            return;

        attendableEvent = new AttendableEvent(@event.AgendaItemId, slot.AgendaItem.ConferenceId, slot.From, slot.To);
        var slotPolicy = slotPolicyFactory.Get(slot.AgendaItem.Tags.ToArray());
        var slots = slotPolicy.Generate(slot.ParticipantsLimit.Value);
        attendableEvent.AddSlots(slots);
        await attendableEventRepository.AddAsync(attendableEvent);
    }
}