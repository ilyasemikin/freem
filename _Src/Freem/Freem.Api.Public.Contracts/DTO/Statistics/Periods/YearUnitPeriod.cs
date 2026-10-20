using Freem.Time.Models;
using Freem.Api.Public.Contracts.DTO.Statistics.Periods.Abstractions;

namespace Freem.Api.Public.Contracts.DTO.Statistics.Periods;

/// <summary>A calendar year.</summary>
public sealed class YearUnitPeriod : IUnitPeriod
{
    /// <inheritdoc />
    public DateUnit Unit { get; } = DateUnit.Year;
    
    /// <summary>The year.</summary>
    public int Year { get; }

    public YearUnitPeriod(int year)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(year);
        
        Year = year;
    }
}