using Confab.Modules.Conferences.Core.Entities;

namespace Confab.Modules.Conferences.Core.Policies;

internal class HostDeletionPolicy(IConferenceDeletionPolicy conferenceDeletionPolicy) : IHostDeletionPolicy
{
    public async Task<bool> CanDeleteAsync(Host host)
    {
        if (host.Conferences is null || !host.Conferences.Any())
        {
            return true;
        }

        foreach (var conference in host.Conferences)
        {
            if (await conferenceDeletionPolicy.CanDeleteAsync(conference) is false)
                return false;
        }

        return true;
    }
}