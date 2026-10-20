using Freem.Time.Models;
using Freem.Api.Public.Contracts.DTO.Statistics.Periods.Abstractions;

namespace Freem.Api.Public.Contracts.DTO.Statistics.Periods;

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