using Confab.Modules.Agendas.Domain.CallForPapers.Exceptions;
using Confab.Shared.Abstractions.Kernel.Types;

namespace Confab.Modules.Agendas.Domain.CallForPapers.Entities;

public sealed class CallForPapers : AggregateRoot
{
    public ConferenceId ConferenceId { get; private set; }
    public DateTime From { get; private set; }
    public DateTime To { get; private set; }
    public bool IsOpened { get; private set; }

    public CallForPapers(AggregateId id, ConferenceId conferenceId, DateTime from, DateTime to, bool isOpened,
        int version = 0)
    {
        Id = id;
        ConferenceId = conferenceId;
        From = from;
        To = to;
        IsOpened = isOpened;
    }

    internal CallForPapers(AggregateId id)
        => Id = id;
    
    private CallForPapers()
    {
    }

    public static CallForPapers Create(AggregateId id, Guid conferenceId, DateTime from, DateTime to)
    {
        var cfp = new CallForPapers(id);
        cfp.ConferenceId = conferenceId;
        cfp.ChangeDateRange(from, to);
        cfp.IsOpened = false;
        cfp.ClearEvents();
        cfp.Version = 0;

        return cfp;
    }

    public void ChangeDateRange(DateTime from, DateTime to)
    {
        if (from.Date > to.Date)
            throw new InvalidCallForPapersDatesException(from, to);

        From = from;
        To = to;
        IncrementVersion();
    }

    public void Open()
    {
        IsOpened = true;
        IncrementVersion();
    }

    public void Close()
    {
        IsOpened = false;
        IncrementVersion();
    }
}