using Confab.Modules.Agendas.Domain.Agendas.Exceptions;
using Confab.Shared.Abstractions.Kernel.Types;

namespace Confab.Modules.Agendas.Domain.Agendas.Entities;

public abstract class AgendaSlot
{
    public EntityId Id { get; protected set; }
    public DateTime From { get; private set; }
    public DateTime To { get; private set; }

    // For EF
    public AgendaTrack Track { get; set; }

    protected AgendaSlot(EntityId id, DateTime from, DateTime to)
    {
        Id = id;
        From = from;
        To = to;
    }

    protected AgendaSlot(EntityId id)
        => Id = id;

    protected AgendaSlot()
    {
    }

    protected void ChangeDateRange(DateTime from, DateTime to)
    {
        if (from.Date > to.Date)
            throw new InvalidAgendaSlotDatesException(from, to);

        From = from;
        To = to;
    }
}