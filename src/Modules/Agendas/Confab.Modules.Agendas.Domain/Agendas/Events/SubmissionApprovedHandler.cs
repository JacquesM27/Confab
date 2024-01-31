using Confab.Modules.Agendas.Domain.Agendas.Entities;
using Confab.Modules.Agendas.Domain.Agendas.Repositories;
using Confab.Modules.Agendas.Domain.Submissions.Consts;
using Confab.Modules.Agendas.Domain.Submissions.Events;
using Confab.Shared.Abstractions.Kernel;

namespace Confab.Modules.Agendas.Domain.Agendas.Events;

internal sealed class SubmissionApprovedHandler(
    IAgendaItemRepository agendaItemRepository
    ) : IDomainEventHandler<SubmissionStatusChanged>
{
    public async Task HandleAsync(SubmissionStatusChanged @event)
    {
        if (@event.Status is SubmissionStatus.Rejected)
            return;

        var submission = @event.Submission;
        var agendaItem = await agendaItemRepository.GetAsync(submission.Id);
        if (agendaItem is not null)
            return;

        agendaItem = AgendaItem.Create(submission.Id, submission.ConferenceId, submission.Title,
            submission.Description, submission.Level, submission.Tags, submission.Speakers.ToList());

        await agendaItemRepository.AddAsync(agendaItem);
    }
}