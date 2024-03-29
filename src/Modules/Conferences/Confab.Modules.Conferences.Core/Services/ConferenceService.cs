﻿using Confab.Modules.Conferences.Core.DTO;
using Confab.Modules.Conferences.Core.Entities;
using Confab.Modules.Conferences.Core.Events;
using Confab.Modules.Conferences.Core.Exception;
using Confab.Modules.Conferences.Core.Policies;
using Confab.Modules.Conferences.Core.Repositories;
using Confab.Shared.Abstractions.Messaging;

namespace Confab.Modules.Conferences.Core.Services;

internal class ConferenceService(
    IConferenceRepository conferenceRepository,
    IHostRepository hostRepository, 
    IConferenceDeletionPolicy conferenceDeletionPolicy,
    IMessageBroker messageBroker
    ) : IConferenceService
{
    public async Task AddAsync(ConferenceDetailsDto dto)
    {
        if (await hostRepository.GetAsync(dto.HostId) is null)
            throw new HostNotFoundException(dto.HostId);

        dto.Id = Guid.NewGuid();
        var conference = new Conference()
        {
            Id = dto.Id,
            HostId = dto.HostId,
            Name = dto.Name,
            Description = dto.Description,
            From = dto.From,
            To = dto.To,
            Location = dto.Location,
            LogoUrl = dto.LogoUrl,
            ParticipantsLimit = dto.ParticipantsLimit,
        };
        await conferenceRepository.AddAsync(conference);

        await messageBroker.PublishAsync(new ConferenceCreated(conference.Id, conference.Name,
            conference.ParticipantsLimit, conference.From, conference.To));
    }

    public async Task<ConferenceDetailsDto?> GetAsync(Guid id)
    {
        var conference = await conferenceRepository.GetAsync(id);

        if (conference is null)
            return null;
        

        var dto = Map<ConferenceDetailsDto>(conference);

        dto.Description = conference.Description;

        return dto;
    }

    public async Task<IReadOnlyList<ConferenceDto>> BrowseAsync()
    {
        var conferences = await conferenceRepository.BrowseAsync();

        return conferences.Select(Map<ConferenceDto>).ToList();
    }

    public async Task UpdateAsync(ConferenceDetailsDto dto)
    {
        var conference = await conferenceRepository.GetAsync(dto.Id) 
            ?? throw new ConferenceNotFoundException(dto.Id);

        conference.Name = dto.Name;
        conference.Description = dto.Description;
        conference.Location = dto.Location;
        conference.LogoUrl = dto.LogoUrl;
        conference.From = dto.From;
        conference.To = dto.To;
        conference.ParticipantsLimit = dto.ParticipantsLimit;
        await conferenceRepository.UpdateAsync(conference);
    }

    public async Task DeleteAsync(Guid id)
    {
        var conference = await conferenceRepository.GetAsync(id) 
            ?? throw new ConferenceNotFoundException(id);

        if (await conferenceDeletionPolicy.CanDeleteAsync(conference) is false)
            throw new CannotDeleteConferenceException(id);

        await conferenceRepository.DeleteAsync(conference);
    }

    private static T Map<T>(Conference conference) where T : ConferenceDto, new()
        => new()
        {
            Id = conference.Id,
            HostId = conference.HostId,
            HostName = conference.Host?.Name,
            Name = conference.Name,
            Location = conference.Location,
            From = conference.From,
            To = conference.To,
            LogoUrl = conference.LogoUrl,
            ParticipantsLimit = conference.ParticipantsLimit
        };
}