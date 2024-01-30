using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Agendas.Domain.Agendas.Exceptions;

internal sealed class EmptyAgendaTrackNameException(Guid agendaTrackId)
    : ConfabException($"Agenda track with ID: '{agendaTrackId}' defines empty name.")
{
}