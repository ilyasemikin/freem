namespace Freem.Timeouts.Models;

public class Timeout
{
    public static readonly TimeSpan DefaultPeriod = TimeSpan.FromSeconds(10);
    public const int DefaultRepeats = 1;

    public TimeSpan Period { get; }
    public int Repeats { get; }

    public Timeout()
        : this(DefaultPeriod)
    {
    }
    
    public Timeout(TimeSpan period, int repeats = DefaultRepeats)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(repeats);
        
        Period = period;
        Repeats = repeats;
    }
}
