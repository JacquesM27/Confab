using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Agendas.Application.Agendas.Exceptions;

internal sealed class AgendaTrackNotFoundException(Guid agendaTrackId) 
    : ConfabException($"Agenda with ID: '{agendaTrackId}' was not found.")
{
    public Guid AgendaTrackId => agendaTrackId;
}