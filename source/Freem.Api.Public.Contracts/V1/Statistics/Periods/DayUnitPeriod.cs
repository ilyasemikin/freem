using Freem.Api.Public.Contracts.V1.Statistics.Periods.Abstractions;
using Freem.Time;

namespace Freem.Api.Public.Contracts.V1.Statistics.Periods;

/// <summary>A single day.</summary>
public sealed class DayUnitPeriod : IUnitPeriod
{
    /// <inheritdoc />
    public DateUnit Unit { get; } = DateUnit.Day;
    
    /// <summary>The day.</summary>
    public DateOnly Day { get; }

    public DayUnitPeriod(DateOnly day)
    {
        Day = day;
    }
}