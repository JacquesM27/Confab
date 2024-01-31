using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Agendas.Domain.Agendas.Exceptions;

internal sealed class InvalidAgendaItemLevelException(Guid agendaItemId)
    : ConfabException($"Agenda item with ID: '{agendaItemId}' defines invalid level.")
{
    public Guid AgendaItemId => agendaItemId;   
}