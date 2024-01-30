using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Agendas.Domain.Agendas.Exceptions;

internal sealed class AgendaSlotNotFoundException(Guid agendaSlotId)
    : ConfabException($"Agenda slot with ID: '{agendaSlotId}' was not found.")
{
    public Guid AgendaSlotId => agendaSlotId;
}