using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Statistics.Time;
using Freem.UseCases.Contracts.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;

namespace Freem.Entities.UseCases.Contracts.Statistics.PerDays;

public sealed class StatisticsPerDaysResponse : IResponse<StatisticsPerDaysErrorCode>
{
    [MemberNotNullWhen(true, nameof(Statistics))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Success { get; }
    
    public IReadOnlyDictionary<DateOnly, TimeStatistics>? Statistics { get; }
    public Error<StatisticsPerDaysErrorCode>? Error { get; }

    private StatisticsPerDaysResponse(
        IReadOnlyDictionary<DateOnly, TimeStatistics>? statistics = null, 
        Error<StatisticsPerDaysErrorCode>? error = null)
    {
        Success = statistics is not null;
        Statistics = statistics;
        Error = error;
    }

    public static StatisticsPerDaysResponse CreateSuccess(IReadOnlyDictionary<DateOnly, TimeStatistics> statistics)
    {
        ArgumentNullException.ThrowIfNull(statistics);
        
        return new StatisticsPerDaysResponse(statistics);
    }

    public static StatisticsPerDaysResponse CreateFailure(StatisticsPerDaysErrorCode code, string? message = null)
    {
        var error = new Error<StatisticsPerDaysErrorCode>(code, message);
        return new StatisticsPerDaysResponse(error: error);
    }
}