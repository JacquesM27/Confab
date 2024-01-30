using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Agendas.Domain.CallForPapers.Exceptions;

internal sealed class InvalidCallForPapersDatesException(DateTime from, DateTime to) 
    : ConfabException($"CFP has invalid dates, from: {from:d} > to: {to:d}")
{
    public DateTime From => from;
    public DateTime To => to;
}