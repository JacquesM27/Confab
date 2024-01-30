using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Agendas.Domain.Agendas.Exceptions;

internal sealed class NegativeParticipantsLimitException(Guid agendaSlotId)
    : ConfabException($"Regular slot with ID: '{agendaSlotId}' defines negative participants limit.")
{
    public Guid AgendaSlotId => agendaSlotId;
}