using Confab.Modules.Attendances.Application.Clients.Agendas;
using Confab.Modules.Attendances.Application.Clients.Agendas.DTO;
using Confab.Modules.Attendances.Infrastructure.Clients.Agendas.Requests;
using Confab.Shared.Abstractions.Modules;

namespace Confab.Modules.Attendances.Infrastructure.Clients.Agendas;

internal sealed class AgendasApiClient(
    IModuleClient moduleClient
    ) : IAgendasApiClient
{
    public Task<RegularAgendaSlotDto> GetRegularAgendaSlotAsync(Guid id)
        => moduleClient.SendAsync<RegularAgendaSlotDto>("agendas/slots/regular/get",
            new GetRegularAgendaSlot
            {
                AgendaItemId = id
            });

    public Task<IEnumerable<AgendaTrackDto>> GetAgendaAsync(Guid conferenceId)
    {
        throw new NotImplementedException();
    }
}