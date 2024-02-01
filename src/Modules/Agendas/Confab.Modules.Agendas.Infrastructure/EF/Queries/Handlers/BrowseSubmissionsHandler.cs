using Confab.Modules.Agendas.Application.Submissions.DTO;
using Confab.Modules.Agendas.Application.Submissions.Queries;
using Confab.Modules.Agendas.Domain.Submissions.Entities;
using Confab.Modules.Agendas.Infrastructure.EF.Mappings;
using Confab.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace Confab.Modules.Agendas.Infrastructure.EF.Queries.Handlers;

internal sealed class BrowseSubmissionsHandler(
    AgendasDbContext dbContext
    ) : IQueryHandler<BrowseSubmissions, IEnumerable<SubmissionDto>>
{
    private readonly DbSet<Submission> _submissions = dbContext.Submissions;

    public async Task<IEnumerable<SubmissionDto>> HandleAsync(BrowseSubmissions query)
    {
        var submissions = _submissions
            .AsNoTracking()
            .Include(s => s.Speakers)
            .AsQueryable();

        if (query.ConferenceId.HasValue)
            submissions = submissions.Where(s => s.ConferenceId == query.ConferenceId);

        if (query.SpeakerId.HasValue)
            submissions = submissions.Where(s => s.Speakers.Any(x => x.Id == query.SpeakerId));

        return await submissions
            .Select(s => s.AsDto())
            .ToListAsync();
    }
}