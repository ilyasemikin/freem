using Freem.Api.Public.Contracts.V1.Statistics.Periods.Abstractions;
using Freem.Time;

namespace Freem.Api.Public.Contracts.V1.Statistics.Periods;

/// <summary>A week; it starts on the user's first day of the week.</summary>
public sealed class WeekUnitPeriod : IUnitPeriod
{
    /// <inheritdoc />
    public DateUnit Unit { get; } = DateUnit.Week;
    
    /// <summary>Any day of the week.</summary>
    public DateOnly Week { get; }

    public WeekUnitPeriod(DateOnly week)
    {
        Week = week;
    }
}
