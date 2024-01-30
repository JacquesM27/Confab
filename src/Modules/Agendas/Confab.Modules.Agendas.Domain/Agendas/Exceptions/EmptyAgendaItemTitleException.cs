using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Agendas.Domain.Agendas.Exceptions;

internal sealed class EmptyAgendaItemTitleException(Guid agendaItemId)
    : ConfabException($"Agenda item with ID: '{agendaItemId}' defines empty title.")
{
    public Guid AgendaItemId => agendaItemId;   
}