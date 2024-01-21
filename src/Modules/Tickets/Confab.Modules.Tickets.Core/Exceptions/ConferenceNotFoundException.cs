using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Tickets.Core.Exceptions;

internal sealed class ConferenceNotFoundException(Guid id) 
    : ConfabException($"Conference with ID: '{id}' was not found.")
{
    public Guid Id => id;
}