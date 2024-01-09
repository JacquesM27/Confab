using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Conferences.Core.Exception;

internal class ConferenceNotFoundException(Guid id) : ConfabException($"Conference with ID: '{id}' was not found.")
{
    public Guid Id => id;
}