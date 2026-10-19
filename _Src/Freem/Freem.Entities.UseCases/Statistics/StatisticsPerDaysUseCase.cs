using Freem.Domain.Entities.Records;
using Freem.Domain.Entities.Statistics.Time;
using Freem.Domain.Entities.Users;
using Freem.Domain.Statistics.Time;
using Freem.Domain.Storage.Repositories.Records;
using Freem.Domain.Users.Identifiers;
using Freem.Entities.UseCases.Contracts.Statistics.PerDays;
using Freem.Entities.UseCases.Contracts.Users.Settings.Get;
using Freem.Time.Extensions;
using Freem.Time.Models;
using Freem.UseCases.Abstractions;

namespace Freem.Entities.UseCases.Statistics;

public sealed class StatisticsPerDaysUseCase : 
    IUseCase<UseCaseExecutionContext, StatisticsPerDaysRequest, StatisticsPerDaysResponse, StatisticsPerDaysErrorCode>
{
    private readonly IRecordRepository _repository;
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;

    public StatisticsPerDaysUseCase(
        IRecordRepository repository, 
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
        // The end of the period is inclusive
        var end = request.Period.EndAt.AddDays(1).ToUtcDateTime(time);

        var statisticsPerDays = new Dictionary<DateOnly, TimeStatistics>();
        while (current < end)
        {
            var records = await FindRecordsAsync(context.UserId, current, cancellationToken);

            var next = current.AddDays(1);
            var period = new DateTimePeriod(current, next);

            var day = current.ToDateOnly();
            statisticsPerDays[day] = TimeStatisticsCalculator.Calculate(period, records);
            
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
            var filter = new RecordByPeriodFilter(userId, period);

            var result = await _repository.FindAsync(filter, cancellationToken);
            var records = result.ToBlockingEnumerable(cancellationToken);
            
            list.AddRange(records);

            if (list.Count >= result.TotalCount)
                return list;
        } while (true);
    }
}