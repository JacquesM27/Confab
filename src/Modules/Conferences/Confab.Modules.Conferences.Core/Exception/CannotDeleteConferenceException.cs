using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Conferences.Core.Exception;

internal class CannotDeleteConferenceException(Guid id) : ConfabException($"Conference with ID: '{id}' cannot be deleted.")
{
    public Guid Id => id;
}