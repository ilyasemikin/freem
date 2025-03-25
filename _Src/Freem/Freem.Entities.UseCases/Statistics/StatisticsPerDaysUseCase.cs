using Freem.Entities.Records;
using Freem.Entities.Records.Identifiers;
using Freem.Entities.Statistics.Time;
using Freem.Entities.Storage.Abstractions.Base.Search;
using Freem.Entities.Storage.Abstractions.Models.Filters;
using Freem.Entities.UseCases.Contracts.Statistics.PerDays;
using Freem.Entities.UseCases.Contracts.Users.Settings.Get;
using Freem.Entities.Users;
using Freem.Entities.Users.Identifiers;
using Freem.Time.Extensions;
using Freem.Time.Models;
using Freem.UseCases.Abstractions;

namespace Freem.Entities.UseCases.Statistics;

public sealed class StatisticsPerDaysUseCase : 
    IUseCase<UseCaseExecutionContext, StatisticsPerDaysRequest, StatisticsPerDaysResponse, StatisticsPerDaysErrorCode>
{
    private readonly IMultipleSearchByFilterRepository<Record, RecordIdentifier, RecordsByPeriodFilter> _repository;
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;

    public StatisticsPerDaysUseCase(
        IMultipleSearchByFilterRepository<Record, RecordIdentifier, RecordsByPeriodFilter> repository, 
        IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(executor);
        
        _repository = repository;
        _executor = executor;
    }

    public async Task<StatisticsPerDaysResponse> ExecuteAsync(
        UseCaseExecutionContext context, StatisticsPerDaysRequest request,
        CancellationToken cancellationToken = default)
    {
        context.ThrowsIfUnauthorized();

        var settings = await GetUserSettingsAsync(context, cancellationToken);
        if (settings is null)
            return StatisticsPerDaysResponse.CreateFailure(StatisticsPerDaysErrorCode.UserNotFound);

        var time = TimeOnly.MinValue.Add(settings.DayUtcOffset);
        var current = request.Period.StartAt.ToUtcDateTime(time);
        var end = request.Period.EndAt.ToUtcDateTime(time);

        var statisticsPerDays = new Dictionary<DateOnly, TimeStatistics>();
        while (current <= end)
        {
            var records = await FindRecordsAsync(context.UserId, current, cancellationToken);

            var next = current.AddDays(1);
            var period = new DateTimePeriod(current, next);

            var day = current.ToDateOnly();
            statisticsPerDays[day] = TimeStatistics.Calculate(period, records);
            
            current = next;
        }

        return StatisticsPerDaysResponse.CreateSuccess(statisticsPerDays);
    }

    private async Task<UserSettings?> GetUserSettingsAsync(
        UseCaseExecutionContext context, CancellationToken cancellationToken)
    {
        var request = new GetUserSettingsRequest();
        var response = await _executor.ExecuteAsync<GetUserSettingsRequest, GetUserSettingsResponse>(context, request, cancellationToken);
        return response.Settings;
    }

    private async Task<IReadOnlyList<Record>> FindRecordsAsync(
        UserIdentifier userId, DateTime start, 
        CancellationToken cancellationToken)
    {
        var list = new List<Record>();
        
        do
        {
            if (list.Count != 0)
            {
                var last = list.Last();
                start =  last.Period.EndAt.DateTime;
            }

            var end = start.AddDays(1);
            var period = new DateTimePeriod(start, end);
            var filter = new RecordsByPeriodFilter(userId, period);

            var result = await _repository.FindAsync(filter, cancellationToken);
            var records = result.ToBlockingEnumerable(cancellationToken);
            
            list.AddRange(records);

            if (list.Count >= result.TotalCount)
                return list;
        } while (true);
    }
}