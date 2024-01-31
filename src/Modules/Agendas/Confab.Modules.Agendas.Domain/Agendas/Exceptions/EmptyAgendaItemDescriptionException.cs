using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Agendas.Domain.Agendas.Exceptions;

internal sealed class EmptyAgendaItemDescriptionException(Guid agendaItemId)
    : ConfabException($"Agenda item with ID: '{agendaItemId}' defines empty description.")
{
    public Guid AgendaItemId => agendaItemId;   
}