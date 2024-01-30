using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Agendas.Domain.Agendas.Exceptions;

internal sealed class EmptyAgendaSlotPlaceholderException()
    : ConfabException("Agenda slot defines empty placeholder.")
{
}