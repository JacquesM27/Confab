using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Speakers.Core.Exceptions;

internal class SpeakerNotFoundException(Guid id) : ConfabException($"Conference with ID: '{id}' was not found.")
{
    public Guid Id => id;
}