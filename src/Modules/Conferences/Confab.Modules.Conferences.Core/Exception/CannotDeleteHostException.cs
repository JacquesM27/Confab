using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Conferences.Core.Exception;

internal class CannotDeleteHostException(Guid id) : ConfabException($"Host with ID: '{id}' cannot be deleted.")
{
    public Guid Id => id;
}