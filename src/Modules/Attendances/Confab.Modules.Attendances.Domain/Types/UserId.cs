using Confab.Shared.Abstractions.Kernel.Types;

namespace Confab.Modules.Attendances.Domain.Types;

public class UserId(Guid value) : TypeId(value)
{
    public static implicit operator UserId(Guid id) => new(id);
    public static implicit operator Guid(UserId id) => id.Value;
}