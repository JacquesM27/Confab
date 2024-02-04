using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Attendances.Domain.Exceptions;

public sealed class ParticipantNotFoundException(Guid conferenceId, Guid participantId)
    : ConfabException($"Participant of conference: '{conferenceId}' with participant ID: '{participantId}' was not found.")
{
    public Guid ConferenceId => conferenceId;
    public Guid ParticipantId => participantId;
}
