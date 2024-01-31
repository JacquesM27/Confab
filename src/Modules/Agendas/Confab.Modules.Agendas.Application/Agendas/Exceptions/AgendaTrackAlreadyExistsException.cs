using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Agendas.Application.Agendas.Exceptions;

internal sealed class AgendaTrackAlreadyExistsException(Guid agendaTrackId)
    : ConfabException($"Agenda track with ID: '{agendaTrackId}' already exists.")
{
    public Guid AgendaTrackId => agendaTrackId;
}