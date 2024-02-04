using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Attendances.Domain.Exceptions;

public class NoFreeSlotsException()
    : ConfabException("No free slots left")
{
    
}