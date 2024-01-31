using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Agendas.Domain.Agendas.Exceptions;

internal sealed class AgendaItemNotFoundException(Guid agendaItemId)
    : ConfabException($"Agenda item with ID: '{agendaItemId}' was not found.")
{
    public Guid AgendaItemId => agendaItemId;
}