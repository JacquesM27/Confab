namespace Confab.Shared.Abstractions.Kernel.Types;

public class ConferenceId(Guid value) : TypeId(value)
{
    public static implicit operator ConferenceId(Guid id) => new(id);
}