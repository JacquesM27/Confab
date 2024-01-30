using Confab.Modules.Agendas.Domain.Agendas.Exceptions;
using Confab.Modules.Agendas.Domain.Submissions.Entities;
using Confab.Shared.Abstractions.Kernel.Types;

namespace Confab.Modules.Agendas.Domain.Agendas.Entities;

public sealed class AgendaItem : AggregateRoot
{
    public ConferenceId ConferenceId { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public int Level { get; private set; }
    public IEnumerable<string> Tags { get; private set; }
    public IEnumerable<Speaker> Speakers => _speakers;
    
    private ICollection<Speaker> _speakers = new List<Speaker>();

    //For EF
    public AgendaSlot AgendaSlot { get; private set; }

    public AgendaItem(AggregateId id, ConferenceId conferenceId, string title, string description,
        int level, IEnumerable<string> tags, ICollection<Speaker> speakers, int version = 0)
    {
        Id = id;
        ConferenceId = conferenceId;
        Title = title;
        Description = description;
        Level = level;
        Tags = tags;
        _speakers = speakers;
        Version = version;
    }

    internal AgendaItem(AggregateId id, ConferenceId conferenceId)
        => (Id, ConferenceId) = (id, conferenceId);

    private AgendaItem()
    {
    }

    public static AgendaItem Create(AggregateId id, ConferenceId conferenceId, string title, string description,
        int level, IEnumerable<string> tags, ICollection<Speaker> speakers)
    {
        var agendaItem = new AgendaItem(id, conferenceId);
        agendaItem.IncrementVersion();

        // TODO: other properties
        
        return agendaItem;
    }

    public void ChangeTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new EmptyAgendaItemTitleException(Id);
        Title = title;
        
        IncrementVersion();
    }
}