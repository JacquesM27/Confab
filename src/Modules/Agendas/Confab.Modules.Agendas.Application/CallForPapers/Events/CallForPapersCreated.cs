using Confab.Shared.Abstractions.Events;

namespace Confab.Modules.Agendas.Application.CallForPapers.Events;

public record CallForPapersCreated(Guid Id, Guid ConferenceId) : IEvent;