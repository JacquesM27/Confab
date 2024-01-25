using Confab.Modules.Speakers.Core.DTO;

namespace Confab.Modules.Speakers.Core.Services;

internal interface ISpeakerService
{
    Task CreateAsync(SpeakerDto dto);
    Task<SpeakerDto?> GetAsync(Guid id);
    Task<IReadOnlyList<SpeakerDto>> BrowseAsync();
    Task UpdateAsync(SpeakerDto dto);
}