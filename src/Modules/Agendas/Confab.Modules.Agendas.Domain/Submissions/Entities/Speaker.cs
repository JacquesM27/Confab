﻿using Confab.Modules.Agendas.Domain.Agendas.Entities;
using Confab.Shared.Abstractions.Kernel.Types;

namespace Confab.Modules.Agendas.Domain.Submissions.Entities;

public class Speaker : AggregateRoot
{
    public string FullName { get; }

    public IEnumerable<Submission> Submissions => _submissions;
    private ICollection<Submission> _submissions;

    public IEnumerable<AgendaItem> AgendaItems => _agendaItems;
    private ICollection<AgendaItem> _agendaItems;
    
    public Speaker(AggregateId id, string fullName)
    {
        Id = id;
        FullName = fullName;
    }

    private Speaker()
    {
    }
    
    public static Speaker Create(Guid id, string fullName)
        => new Speaker(id, fullName);
}