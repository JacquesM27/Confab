using Confab.Modules.Agendas.Application.Agendas.DTO;
using Confab.Shared.Abstractions.Queries;

namespace Confab.Modules.Agendas.Application.Agendas.Queries;

public sealed class GetRegularAgendaSlot : IQuery<RegularAgendaSlotDto>
{
    public Guid AgendaItemId { get; set; }
}