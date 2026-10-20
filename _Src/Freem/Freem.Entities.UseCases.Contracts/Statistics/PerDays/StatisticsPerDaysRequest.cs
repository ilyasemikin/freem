using Freem.Time.Models;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Entities.UseCases.Contracts.Statistics.PerDays;

public sealed class StatisticsPerDaysRequest : IRequest
{
    public RequestType Type => RequestTypes.Statistics.PerDays;

    public DatePeriod Period { get; }

    public StatisticsPerDaysRequest(DatePeriod period)
    {
        ArgumentNullException.ThrowIfNull(period);
        
        Period = period;
    }
}