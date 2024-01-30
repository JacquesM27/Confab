using Confab.Modules.Agendas.Application.CallForPapers.DTO;
using Confab.Modules.Agendas.Application.CallForPapers.Queries;
using Confab.Modules.Agendas.Infrastructure.EF.Mappings;
using Confab.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace Confab.Modules.Agendas.Infrastructure.EF.Queries.Handlers;

internal sealed class GetCallForPapersHandler(AgendasDbContext context) : IQueryHandler<GetCallForPapers, CallForPapersDto?>
{
    public async Task<CallForPapersDto?> HandleAsync(GetCallForPapers query)
        => await context.CallForPapers
            .AsNoTracking()
            .Where(cfp => cfp.ConferenceId == query.ConferenceId)
            .Select(cfp => cfp.AsDto())
            .SingleOrDefaultAsync();
}