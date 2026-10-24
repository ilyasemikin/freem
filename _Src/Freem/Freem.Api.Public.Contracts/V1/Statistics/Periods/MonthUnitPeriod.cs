using Freem.Api.Public.Contracts.V1.Statistics.Periods.Abstractions;
using Freem.Time;

namespace Freem.Api.Public.Contracts.V1.Statistics.Periods;

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