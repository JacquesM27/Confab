using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Agendas.Domain.Agendas.Exceptions;

internal sealed class ConflictingAgendaSlotsException(DateTime from, DateTime to)
    : ConfabException($"There is slot conflicting with date range: '{from:d}' | '{to:d}'.")
{
    public DateTime From => from;
    public DateTime To => to;
}