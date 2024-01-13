using Confab.Modules.Speakers.Core.DTO;
using Confab.Modules.Speakers.Core.Entities;
using Confab.Modules.Speakers.Core.Exceptions;
using Confab.Modules.Speakers.Core.Repositories;

namespace Confab.Modules.Speakers.Core.Services;

internal class SpeakerService(ISpeakerRepository speakerRepository) : ISpeakerService
{
    public async Task AddAsync(SpeakerDto dto)
    {
        dto.Id = Guid.NewGuid();
        await speakerRepository.AddAsync(new Speaker()
        {
            AvatarUrl = dto.AvatarUrl,
            Bio = dto.Bio,
            Email = dto.Email,
            FullName = dto.FullName,
            Id = dto.Id
        });
    }

    public async Task<SpeakerDto?> GetAsync(Guid id)
    {
        var speaker = await speakerRepository.GetAsync(id);

        if (speaker is null)
            return null;

        var dto = Map<SpeakerDto>(speaker);

        return dto;
    }

    public async Task<IReadOnlyList<SpeakerDto>> BrowseAsync()
    {
        var speakers = await speakerRepository.BrowseAsync();
        return speakers.Select(Map<SpeakerDto>).ToList();
    }

    public async Task UpdateAsync(SpeakerDto dto)
    {
        var speaker = await speakerRepository.GetAsync(dto.Id)
            ?? throw new SpeakerNotFoundException(dto.Id);

        speaker.AvatarUrl = dto.AvatarUrl;
        speaker.Bio = dto.Bio;
        speaker.Email = dto.Email;
        speaker.FullName = dto.FullName;

        await speakerRepository.UpdateAsync(speaker);
    }

    public async Task DeleteAsync(Guid id)
    {
        var speaker = await speakerRepository.GetAsync(id)
            ?? throw new SpeakerNotFoundException(id);

        await speakerRepository.DeleteAsync(speaker);
    }

    private static T Map<T>(Speaker speaker) where T : SpeakerDto, new()
        => new()
        {
            Id = speaker.Id,
            Bio = speaker.Bio,
            Email = speaker.Email,
            FullName = speaker.FullName,
            AvatarUrl = speaker.AvatarUrl
        };
}