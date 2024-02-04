using Confab.Shared.Abstractions.Kernel.Types;

namespace Confab.Modules.Attendances.Domain.Types;

public class SlotId(Guid value) : TypeId(value)
{
    public static implicit operator SlotId(Guid id) => new(id);
    public static implicit operator Guid(SlotId id) => id.Value;
}