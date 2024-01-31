using Confab.Shared.Abstractions.Commands;

namespace Confab.Modules.Agendas.Application.Agendas.Commands;

public record AssignRegularAgendaSlot(Guid AgendaTrackId, Guid AgendaSlotId, Guid AgendaItemId) : ICommand;