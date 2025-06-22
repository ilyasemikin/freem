using Freem.Time.Models;
using Freem.Web.Api.Public.Contracts.DTO.Statistics.Periods.Abstractions;

namespace Freem.Web.Api.Public.Contracts.DTO.Statistics.Periods;

public sealed class MonthUnitPeriod : IUnitPeriod
{
    public DateUnit Unit { get; } = DateUnit.Month;
    
    public MonthOnly Month { get; }

    public MonthUnitPeriod(MonthOnly month)
    {
        ArgumentNullException.ThrowIfNull(month);
        
        Month = month;
    }
}