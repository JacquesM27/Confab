using Confab.Modules.Speakers.Core.DTO;
using Confab.Modules.Speakers.Core.Entities;
using Confab.Modules.Speakers.Core.Events;
using Confab.Modules.Speakers.Core.Exceptions;
using Confab.Modules.Speakers.Core.Repositories;
using Confab.Shared.Abstractions.Messaging;

namespace Confab.Modules.Speakers.Core.Services;

internal class SpeakerService(ISpeakerRepository speakerRepository, IMessageBroker messageBroker) : ISpeakerService
{
    public async Task CreateAsync(SpeakerDto dto)
    {
        var alreadyExists = await speakerRepository.ExistsAsync(dto.Id);
        if (alreadyExists)
            throw new SpeakerAlreadyExistsException(dto.Id);

        var speaker = new Speaker()
        {
            AvatarUrl = dto.AvatarUrl,
            Bio = dto.Bio,
            Email = dto.Email,
            FullName = dto.FullName,
            Id = dto.Id
        };
        await speakerRepository.AddAsync(speaker);

        await messageBroker.PublishAsync(new SpeakerCreated(speaker.Id, speaker.FullName));
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