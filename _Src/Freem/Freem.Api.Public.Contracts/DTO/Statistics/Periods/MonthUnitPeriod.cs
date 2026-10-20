using Freem.Time.Models;
using Freem.Api.Public.Contracts.DTO.Statistics.Periods.Abstractions;

namespace Freem.Api.Public.Contracts.DTO.Statistics.Periods;

/// <summary>A calendar month.</summary>
public sealed class MonthUnitPeriod : IUnitPeriod
{
    /// <inheritdoc />
    public DateUnit Unit { get; } = DateUnit.Month;
    
    /// <summary>The month.</summary>
    public MonthOnly Month { get; }

    public MonthUnitPeriod(MonthOnly month)
    {
        ArgumentNullException.ThrowIfNull(month);
        
        Month = month;
    }
}