using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Agendas.Domain.Agendas.Exceptions;

internal sealed class CollidingSpeakerAgendaSlotsException(Guid agendaSlotId, Guid agendaItemId)
    : ConfabException($"Cannot assign agenda item with ID: '{agendaItemId}' to slot with ID: '{agendaSlotId}'.")
{
    
}