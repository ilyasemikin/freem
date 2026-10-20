using Freem.Time.Models;
using Freem.Api.Public.Contracts.DTO.Statistics.Periods.Abstractions;

namespace Freem.Api.Public.Contracts.DTO.Statistics.Periods;

public sealed class DayUnitPeriod : IUnitPeriod
{
    public DateUnit Unit { get; } = DateUnit.Day;
    
    public DateOnly Day { get; }

    public DayUnitPeriod(DateOnly day)
    {
        Day = day;
    }
}