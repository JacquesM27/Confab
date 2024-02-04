using Confab.Modules.Attendances.Domain.Repositories;
using Confab.Modules.Attendances.Domain.Exceptions;
using Confab.Shared.Abstractions.Commands;

namespace Confab.Modules.Attendances.Application.Commands.Handlers;

internal sealed class AttendEventHandler(
    IAttendableEventRepository attendableEventRepository,
    IParticipantsRepository participantsRepository
    ) : ICommandHandler<AttendEvent>
{
    public async Task HandleAsync(AttendEvent command)
    {
        var attendableEvent = await attendableEventRepository.GetAsync(command.Id)
                              ?? throw new AttendableEventNotFoundException(command.Id);

        var participant = await participantsRepository.GetAsync(attendableEvent.ConferenceId, command.ParticipantId)
                          ?? throw new ParticipantNotFoundException(attendableEvent.ConferenceId, command.ParticipantId);

        attendableEvent.Attend(participant);
        await participantsRepository.UpdateAsync(participant);
        await attendableEventRepository.UpdateAsync(attendableEvent);
    }
}