using Freem.Time.Abstractions;

namespace Freem.Time.Implementations;

public class UtcCurrentTimeGetter : ICurrentTimeGetter
{
    public DateTimeOffset Get()
    {
        return DateTimeOffset.UtcNow;
    }
}