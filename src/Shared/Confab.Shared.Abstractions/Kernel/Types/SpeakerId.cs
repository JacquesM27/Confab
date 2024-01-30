namespace Confab.Shared.Abstractions.Kernel.Types;

public class SpeakerId(Guid value) : TypeId(value)
{
    public static implicit operator SpeakerId(Guid id) => new(id);
    public static implicit operator Guid(SpeakerId id) => id.Value;
}