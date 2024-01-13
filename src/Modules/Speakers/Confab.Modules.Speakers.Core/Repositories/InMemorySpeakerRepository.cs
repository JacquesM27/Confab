using Confab.Modules.Speakers.Core.Entities;

namespace Confab.Modules.Speakers.Core.Repositories;

internal class InMemorySpeakerRepository : ISpeakerRepository
{
    private readonly List<Speaker> _speakers = [];

    public Task<Speaker?> GetAsync(Guid id)
        => Task.FromResult(_speakers.SingleOrDefault(x => x.Id == id));

    public async Task<IReadOnlyList<Speaker>> BrowseAsync()
    {
        await Task.CompletedTask;
        return _speakers;
    }

    public Task AddAsync(Speaker speaker)
    {
        _speakers.Add(speaker);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Speaker speaker)
    {
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Speaker speaker)
    {
        _speakers.Remove(speaker);
        return Task.CompletedTask;
    }
}