using Confab.Shared.Abstractions.Kernel.Types;

namespace Confab.Modules.Agendas.Domain.Submissions.Entities;

public class Speaker : AggregateRoot
{
    public string FullName { get; }

    public Speaker(AggregateId id, string fullName)
    {
        Id = id;
        FullName = fullName;
    }

    public static Speaker Create(Guid id, string fullName)
        => new Speaker(id, fullName);
}