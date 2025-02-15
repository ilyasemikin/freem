using Freem.Time.Abstractions;

namespace Freem.Time.Implementations;

public class UtcCurrentTimeGetter : ICurrentTimeGetter
{
    public DateTimeOffset Get()
    {
        var utc = DateTimeOffset.UtcNow;
        return DateTimeOperations.EraseMilliseconds(utc);
    }
}