using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Agendas.Application.Agendas.Exceptions;

internal sealed class AgendaSlotTypeNotFoundException(string type)
    : ConfabException($"Agenda slot type: '{type}' was not found.")
{
    public string Type => type;
}