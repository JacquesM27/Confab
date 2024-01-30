using Confab.Modules.Agendas.Domain.CallForPapers.Entities;
using Confab.Modules.Agendas.Domain.CallForPapers.Repositories;
using Confab.Shared.Abstractions.Kernel.Types;
using Microsoft.EntityFrameworkCore;

namespace Confab.Modules.Agendas.Infrastructure.EF.Repositories;

internal sealed class CallForPapersRepository(AgendasDbContext context) : ICallForPapersRepository
{
    private readonly DbSet<CallForPapers> _callForPapers = context.CallForPapers;

    public async Task<CallForPapers?> GetAsync(ConferenceId conferenceId)
        => await _callForPapers.SingleOrDefaultAsync(cfp => cfp.ConferenceId == conferenceId);
    
    public async Task<bool> ExistsAsync(ConferenceId conferenceId)
        => await _callForPapers.AnyAsync(cfp => cfp.ConferenceId == conferenceId);

    public async Task AddAsync(CallForPapers callForPapers)
    {
        await _callForPapers.AddAsync(callForPapers);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CallForPapers callForPapers)
    {
        _callForPapers.Update(callForPapers);
        await context.SaveChangesAsync();
    }
}