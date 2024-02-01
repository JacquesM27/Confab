using Confab.Modules.Agendas.Application.Agendas.DTO;
using Confab.Shared.Abstractions.Queries;

namespace Confab.Modules.Agendas.Application.Agendas.Queries;

public sealed class GetAgenda : IQuery<IEnumerable<AgendaTrackDto>>
{
    public Guid ConferenceId { get; set; }
}