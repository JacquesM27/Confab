using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Users.Core.Exceptions;

internal sealed class InvalidCredentialsException() : ConfabException("Invalid credentials.")
{
    
}