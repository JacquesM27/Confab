using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Attendances.Domain.Exceptions;

public class AlreadyParticipatingInEventException() 
    : ConfabException($"Already participating in the selected agenda item.")
{
    
}