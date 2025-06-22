using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Statistics.Time;
using Freem.UseCases.Contracts.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;

namespace Freem.Entities.UseCases.Contracts.Statistics.PerPeriod;

public sealed class StatisticsPerPeriodResponse : IResponse<StatisticsPerPeriodErrorCode>
{
    [MemberNotNullWhen(true, nameof(Statistics))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Success { get; }

    public TimeStatistics? Statistics { get; }

    public Error<StatisticsPerPeriodErrorCode>? Error { get; }

    private StatisticsPerPeriodResponse(
        TimeStatistics? statistics = null,
        Error<StatisticsPerPeriodErrorCode>? error = null)
    {
        Success = statistics is not null;
        Statistics = statistics;
        Error = error;
    }

    public static StatisticsPerPeriodResponse CreateSuccess(
        TimeStatistics statistics)
    {
        ArgumentNullException.ThrowIfNull(statistics);

        return new StatisticsPerPeriodResponse(statistics);
    }

    public static StatisticsPerPeriodResponse CreateUsersNotFoundResult()
    {
        var error = new Error<StatisticsPerPeriodErrorCode>(StatisticsPerPeriodErrorCode.UserNotFound);
        return new StatisticsPerPeriodResponse(error: error);
    }
}