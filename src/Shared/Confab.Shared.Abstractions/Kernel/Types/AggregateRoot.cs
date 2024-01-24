namespace Confab.Shared.Abstractions.Kernel.Types;

public abstract class AggregateRoot<TKey>
{ 
    public TKey Id { get; protected set; }
    public int Version { get; protected set; }
    public IEnumerable<IDomainEvent> Events => _events;

    private readonly List<IDomainEvent> _events = [];
    private bool _versionIncremented;
    
    public void ClearEvents() 
        => _events.Clear();
    
    protected void AddEvent(IDomainEvent @event)
    {
        if (_events.Count == 0 && !_versionIncremented)
        {
            Version++;
            _versionIncremented = true;
        }
        
        
        _events.Add(@event);
    }

    protected void IncrementVersion()
    {
        if (_versionIncremented)
            return;
        
        Version++;
        _versionIncremented = true;
    } 
}

public abstract class AggregateRoot : AggregateRoot<Guid>
{
    
}