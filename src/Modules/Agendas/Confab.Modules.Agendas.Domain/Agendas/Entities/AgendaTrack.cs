using Confab.Modules.Agendas.Domain.Agendas.Exceptions;
using Confab.Shared.Abstractions.Kernel.Types;

namespace Confab.Modules.Agendas.Domain.Agendas.Entities;

public sealed class AgendaTrack : AggregateRoot
{
    public ConferenceId ConferenceId { get; private set; }
    public string Name { get; private set; }
    public IEnumerable<AgendaSlot> Slots => _slots;

    private readonly ICollection<AgendaSlot> _slots = new List<AgendaSlot>();

    public AgendaTrack(AggregateId id, ConferenceId conferenceId, string name, 
        IEnumerable<AgendaSlot> slots, int version = 0) : this(id, conferenceId)
    {
        Id = id;
        ConferenceId = conferenceId;
        Name = name;
        _slots = slots.ToHashSet();
        Version = version;
    }

    internal AgendaTrack(AggregateId id, ConferenceId conferenceId)
    {
        Id = id;
        ConferenceId = conferenceId;
    }

    private AgendaTrack()
    {
    }

    public static AgendaTrack Create(AggregateId id, ConferenceId conferenceId, string name)
    {
        var agendaTrack = new AgendaTrack(id, conferenceId);
        agendaTrack.ChangeName(name);
        agendaTrack.ClearEvents();
        agendaTrack.Version = 0;

        return agendaTrack;
    }

    public void ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new EmptyAgendaTrackNameException(Id);

        Name = name;
        IncrementVersion();
    }

    public void AddRegularSlot(EntityId id, DateTime from, DateTime to, int? participantsLimit)
    {
        ValidateTimeConflict(from, to);

        var regularSlot = RegularAgendaSlot.Create(id, from, to, participantsLimit);
        _slots.Add(regularSlot);
        
        IncrementVersion();
    }

    public void AddPlaceholderSlot(EntityId id, DateTime from, DateTime to)
    {
        ValidateTimeConflict(from, to);

        var placeholderSlot = PlaceholderAgendaSlot.Create(id, from, to);
        _slots.Add(placeholderSlot);
        
        IncrementVersion();
    }

    public void ChangeSlotPlaceholder(EntityId id, string placeholder)
    {
        var slot = GetSlot(id);

        if (slot is not PlaceholderAgendaSlot placeholderAgendaSlot)
            throw new InvalidAgendaSlotTypeException(id);
        
        placeholderAgendaSlot.ChangePlaceholder(placeholder);
        
        IncrementVersion();
    }

    public void ChangeSlotAgendaItem(EntityId id, AgendaItem agendaItem)
    {
        var slot = GetSlot(id);

        if (slot is not RegularAgendaSlot regularAgendaSlot)
            throw new InvalidAgendaSlotTypeException(id);
        
        regularAgendaSlot.ChangeAgendaItem(agendaItem);
        
        IncrementVersion();
    }

    public void DeleteSlot(EntityId id)
    {
        var slot = GetSlot(id);

        _slots.Remove(slot);
        IncrementVersion();
    }
    
    private AgendaSlot GetSlot(EntityId id)
        => _slots.SingleOrDefault(x => x.Id == id) ?? throw new AgendaSlotNotFoundException(id);

    private void ValidateTimeConflict(DateTime from, DateTime to)
    {
        var hasConflictingSlots = _slots
            .Any(x => (x.From <= from && x.To >= from) || (x.From <= to && x.To >= to));

        if (hasConflictingSlots is true)
            throw new ConflictingAgendaSlotsException(from, to);
    }
}