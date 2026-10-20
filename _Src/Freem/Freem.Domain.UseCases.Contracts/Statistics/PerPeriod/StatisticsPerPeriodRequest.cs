using Freem.Domain.UseCases.Contracts.Statistics.PerPeriod.Periods.Abstractions;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts.Statistics.PerPeriod;

public sealed class StatisticsPerPeriodRequest : IRequest
{
    public RequestType Type => RequestTypes.Statistics.PerPeriod;

    public IStatisticsPeriod Period { get; }

    public StatisticsPerPeriodRequest(IStatisticsPeriod period)
    {
        ArgumentNullException.ThrowIfNull(period);
        
        Period = period;
    }
}