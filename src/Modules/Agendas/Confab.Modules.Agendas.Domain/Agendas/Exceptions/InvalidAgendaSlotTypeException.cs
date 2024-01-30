using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Agendas.Domain.Agendas.Exceptions;

internal sealed class InvalidAgendaSlotTypeException(Guid agendaSlotId)
    : ConfabException($"Agenda slot with ID: '{agendaSlotId}' has type which does not allow " +
                      $"to perform desired action.")
{
    public Guid AgendaSlotId => agendaSlotId;
}