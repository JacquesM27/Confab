using Confab.Modules.Attendances.Domain.Entities;
using Confab.Modules.Attendances.Domain.Repositories;
using Confab.Modules.Attendances.Domain.Types;
using Microsoft.EntityFrameworkCore;

namespace Confab.Modules.Attendances.Infrastructure.EF.Repositories;

internal sealed class AttendableEventsRepository(
    AttendancesDbContext context
    ) : IAttendableEventRepository
{
    private readonly DbSet<AttendableEvent> _attendableEvents = context.AttendableEvents;

    public Task<AttendableEvent> GetAsync(AttendableEventId id)
        => _attendableEvents
            .Include(x => x.Slots)
            .SingleOrDefaultAsync(s => s.Id == id);

    public async Task AddAsync(AttendableEvent attendableEvent)
    {
        await _attendableEvents.AddAsync(attendableEvent);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(AttendableEvent attendableEvent)
    {
        _attendableEvents.Update(attendableEvent);
        await context.SaveChangesAsync();
    }
}