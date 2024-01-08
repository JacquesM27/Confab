using Confab.Modules.Conferences.Core.Entities;

namespace Confab.Modules.Conferences.Core.Repositories;

internal class InMemoryHostRepository : IHostRepository
{
    private readonly List<Host> _hosts = [];

    public Task<Host?> GetAsync(Guid id) => Task.FromResult(_hosts.SingleOrDefault(x => x.Id == id));

    public async Task<IReadOnlyList<Host>> BrowseAsync()
    {
        await Task.CompletedTask;
        return _hosts;
    }

    public Task AddAsync(Host host)
    {
        _hosts.Add(host);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Host host)
    {
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Host host)
    {
        _hosts.Remove(host);
        return Task.CompletedTask;
    }
}