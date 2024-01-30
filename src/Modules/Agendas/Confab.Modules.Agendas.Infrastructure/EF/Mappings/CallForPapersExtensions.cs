using Confab.Modules.Agendas.Application.CallForPapers.DTO;
using Confab.Modules.Agendas.Domain.CallForPapers.Entities;

namespace Confab.Modules.Agendas.Infrastructure.EF.Mappings;

internal static class CallForPapersExtensions
{
    internal static CallForPapersDto AsDto(this CallForPapers cfp)
        => new()
        {
            Id = cfp.Id,
            ConferenceId = cfp.ConferenceId,
            From = cfp.From,
            To = cfp.To,
            IsOpened = cfp.IsOpened
        };
}