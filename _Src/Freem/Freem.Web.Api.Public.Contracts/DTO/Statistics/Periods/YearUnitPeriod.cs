using Freem.Time.Models;
using Freem.Web.Api.Public.Contracts.DTO.Statistics.Periods.Abstractions;

namespace Freem.Web.Api.Public.Contracts.DTO.Statistics.Periods;

public sealed class YearUnitPeriod : IUnitPeriod
{
    public DateUnit Unit { get; } = DateUnit.Year;
    
    public int Year { get; }

    public YearUnitPeriod(int year)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(year);
        
        Year = year;
    }
}