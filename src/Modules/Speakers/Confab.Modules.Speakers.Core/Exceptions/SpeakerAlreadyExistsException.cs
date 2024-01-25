using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Speakers.Core.Exceptions;

internal sealed class SpeakerAlreadyExistsException(Guid id) : ConfabException($"Speaker with ID: '{id}' already exists.")
{
    public Guid Id => id;
}