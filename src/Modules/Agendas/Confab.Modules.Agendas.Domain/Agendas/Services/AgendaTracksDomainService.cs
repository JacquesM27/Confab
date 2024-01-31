using Confab.Modules.Agendas.Domain.Agendas.Entities;
using Confab.Modules.Agendas.Domain.Agendas.Exceptions;
using Confab.Modules.Agendas.Domain.Agendas.Repositories;
using Confab.Shared.Abstractions.Kernel.Types;

namespace Confab.Modules.Agendas.Domain.Agendas.Services;

public sealed class AgendaTracksDomainService(
    IAgendaTracksRepository agendaTracksRepository,
    IAgendaItemRepository agendaItemRepository
    ) : IAgendaTracksDomainService
{
    public async Task AssignAgendaItemAsync(AgendaTrack agendaTrack, EntityId agendaSlotId, EntityId agendaItemId)
    {

        var slotToAssign = agendaTrack.Slots
            .OfType<RegularAgendaSlot>()
            .SingleOrDefault(s => s.Id == agendaSlotId);

        if (slotToAssign is null)
            throw new AgendaSlotNotFoundException(agendaSlotId);

        var agendaItem = await agendaItemRepository.GetAsync((Guid)agendaItemId);

        if (agendaItem is null)
            throw new AgendaItemNotFoundException(agendaItemId);

        var speakerIds = agendaItem.Speakers.Select(x => new SpeakerId(x.Id));
        var speakerItems = await agendaItemRepository.BrowseAsync(speakerIds);
        var speakerItemIds = speakerItems.Select(s => s.Id).ToList();
        var agendaTracks = await agendaTracksRepository.BrowseAsync(agendaTrack.ConferenceId);

        var hasCollidingSpeakerSlots = agendaTracks
            .SelectMany(track => track.Slots)
            .OfType<RegularAgendaSlot>()
            .Any(slot => speakerItemIds.Contains(slot.Id)
                         && slot.From < slotToAssign.To && slotToAssign.From > slot.To);
        if (hasCollidingSpeakerSlots is true)
            throw new CollidingSpeakerAgendaSlotsException(agendaSlotId, agendaItemId);
        
        agendaTrack.ChangeSlotAgendaItem(agendaSlotId, agendaItem);
    }
}