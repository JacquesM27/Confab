using Confab.Modules.Conferences.Core.DTO;
using Confab.Modules.Conferences.Core.Entities;
using Confab.Modules.Conferences.Core.Exception;
using Confab.Modules.Conferences.Core.Policies;
using Confab.Modules.Conferences.Core.Repositories;

namespace Confab.Modules.Conferences.Core.Services;

class HostService(IHostRepository hostRepository, IHostDeletionPolicy hostDeletionPolicy) : IHostService
{
    public async Task AddAsync(HostDto dto)
    {
        dto.Id = Guid.NewGuid();
        await hostRepository.AddAsync(new Host()
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description
        });
    }

    public async Task<HostDetailsDto?> GetAsync(Guid id)
    {
        var host = await hostRepository.GetAsync(id);

        if (host is null)
            return null;
        
        var dto = Map<HostDetailsDto>(host);

        dto.Conferences = host.Conferences.Select(x => new ConferenceDto()
        {
            Id = x.Id,
            HostId = x.Id,
            HostName = x.Host.Name,
            From = x.From,
            To = x.To,
            Name = x.Name,
            Location = x.Location,
            LogoUrl = x.LogoUrl,
            ParticipantsLimit = x.ParticipantsLimit
        }).ToList();

        return dto;
    }

    public async Task<IReadOnlyList<HostDto>> BrowseAsync()
    {
        var hosts = await hostRepository.BrowseAsync();

        return hosts.Select(Map<HostDto>).ToList();
    }

    public async Task UpdateAsync(HostDetailsDto dto)
    {
        var host = await hostRepository.GetAsync(dto.Id) 
                   ?? throw new HostNotFoundException(dto.Id);

        host.Name = dto.Name;
        host.Description = dto.Description;
        await hostRepository.UpdateAsync(host);
    }

    public async Task DeleteAsync(Guid id)
    {
        var host = await hostRepository.GetAsync(id) 
                   ?? throw new HostNotFoundException(id);

        if (await hostDeletionPolicy.CanDeleteAsync(host) is false)
            throw new CannotDeleteHostException(host.Id);

        await hostRepository.DeleteAsync(host);
    }

    private static T Map<T>(Host host) where T : HostDto, new()
        => new T()
        {
            Id = host.Id,
            Name = host.Name,
            Description = host.Description
        }; 
}