using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Attendances.Domain.Exceptions;

public sealed class AttendableEventNotFoundException(Guid id)
    : ConfabException($"Attendable event with ID: '{id}' was not found.")
{
    public Guid Id => id;
}