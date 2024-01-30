using Confab.Shared.Abstractions.Kernel.Types;

namespace Confab.Modules.Agendas.Domain.CallForPapers.Repositories;

public interface ICallForPapersRepository
{
    Task<Entities.CallForPapers?> GetAsync(ConferenceId conferenceId);
    Task<bool> ExistsAsync(ConferenceId conferenceId);
    Task AddAsync(Entities.CallForPapers callForPapers);
    Task UpdateAsync(Entities.CallForPapers callForPapers);
}