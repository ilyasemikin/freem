using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Statistics;
using Freem.UseCases.Contracts.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;

namespace Freem.Entities.UseCases.Contracts.Statistics.PerDays;

public sealed class StatisticsPerDaysResponse : IResponse<StatisticsPerDaysErrorCode>
{
    [MemberNotNullWhen(true, nameof(Statistics))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Success { get; }
    
    public IReadOnlyDictionary<DateOnly, StatisticsPerDay>? Statistics { get; }
    public Error<StatisticsPerDaysErrorCode>? Error { get; }

    private StatisticsPerDaysResponse(
        IReadOnlyDictionary<DateOnly, StatisticsPerDay>? statistics = null, 
        Error<StatisticsPerDaysErrorCode>? error = null)
    {
        Success = statistics is not null;
        Statistics = statistics;
        Error = error;
    }

    public static StatisticsPerDaysResponse CreateSuccess(IReadOnlyDictionary<DateOnly, StatisticsPerDay> statistics)
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