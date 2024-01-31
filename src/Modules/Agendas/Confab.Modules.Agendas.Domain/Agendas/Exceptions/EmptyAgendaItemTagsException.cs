using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Agendas.Domain.Agendas.Exceptions;

internal sealed class EmptyAgendaItemTagsException(Guid agendaItemId)
    : ConfabException($"Agenda item with ID: '{agendaItemId}' defines empty tags.")
{
    public Guid AgendaItemId => agendaItemId;   
}