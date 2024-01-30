using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Agendas.Domain.Agendas.Exceptions;

internal sealed class InvalidAgendaSlotDatesException(DateTime from, DateTime to)
    : ConfabException($"Agenda slot has invalid dates, from: '{from:d}' > to: '{to:d}'.")
{
    public DateTime From => from;
    public DateTime To => to;
}