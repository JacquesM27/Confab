using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Conferences.Core.Exception;

internal class HostNotFoundException(Guid id) : ConfabException($"Host with ID: '{id}' was not found.")
{
    public Guid Id => id;
}