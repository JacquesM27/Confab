using Confab.Modules.Conferences.Core.Entities;
using Confab.Shared.Abstractions;

namespace Confab.Modules.Conferences.Core.Policies;

internal class ConferenceDeletionPolicy(IClock clock) : IConferenceDeletionPolicy
{
    public Task<bool> CanDeleteAsync(Conference conference)
    {
        // TODO: Check if there are participants.
        var canDelete = clock.CurrentDate().Date.AddDays(7) < conference.From.Date;
        return Task.FromResult(canDelete);
    }
}