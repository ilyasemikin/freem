using Freem.Time;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts.Statistics.PerDays;

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