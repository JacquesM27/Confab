using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Users.Core.Exceptions;

internal sealed class UserNotActiveException(Guid userId) : ConfabException($"User with ID : '{userId}' is not active.")
{
    public Guid UserId => userId;
}