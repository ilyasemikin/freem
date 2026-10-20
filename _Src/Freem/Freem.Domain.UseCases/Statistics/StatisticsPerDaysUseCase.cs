using Freem.Domain.Entities.Records;
using Freem.Domain.Records;
using Freem.Domain.Entities.Statistics.Time;
using Freem.Domain.Entities.Users;
using Freem.Domain.Statistics.Time;
using Freem.Domain.Storage.Repositories.Records;
using Freem.Domain.Users;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Statistics.PerDays;
using Freem.Domain.UseCases.Contracts.Users.Settings.Get;
using Freem.Time.Models;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Domain.UseCases.Statistics;

public sealed class StatisticsPerDaysUseCase : 
    IRequestHandler<HandlerContext, StatisticsPerDaysRequest, Result<StatisticsPerDaysResponse, Error>>
{
    private readonly IRecordRepository _repository;
    private readonly IRequestMediator<HandlerContext> _mediator;

    public StatisticsPerDaysUseCase(
        IRecordRepository repository, 
        IRequestMediator<HandlerContext> mediator)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(mediator);
        
        _repository = repository;
        _mediator = mediator;
    }

    public async Task<Result<StatisticsPerDaysResponse, Error>> HandleAsync(
        HandlerContext context, StatisticsPerDaysRequest request,
        CancellationToken cancellationToken = default)
    {
        var settingsResult = await GetUserSettingsAsync(context, cancellationToken);
        if (!settingsResult.Success)
            return settingsResult.Error;

        var settings = settingsResult.Value;

        var offset = settings.DayUtcOffset;

        var statisticsPerDays = new Dictionary<DateOnly, TimeStatistics>();
        // The end of the period is inclusive
        for (var day = request.Period.StartAt; day <= request.Period.EndAt; day = day.AddDays(1))
        {
            var start = offset.GetDayStartUtc(day);
            var recordsResult = await FindRecordsAsync(context.UserId, start, cancellationToken);
            if (!recordsResult.Success)
                return recordsResult.Error;

            var records = recordsResult.Value;

            var end = start.AddDays(1);
            var period = new DateTimePeriod(start, end);

            statisticsPerDays[day] = TimeStatisticsCalculator.Calculate(period, records);
        }

        return new StatisticsPerDaysResponse(statisticsPerDays);
    }

    private async Task<Result<UserSettings, Error>> GetUserSettingsAsync(
        HandlerContext context, CancellationToken cancellationToken)
    {
        var request = new GetUserSettingsRequest();
        var result = await _mediator.ExecuteAsync<GetUserSettingsRequest, Result<GetUserSettingsResponse, Error>>(context, request, cancellationToken);
        if (!result.Success)
            return result.Error;

        return result.Value.Settings;
    }

    private async Task<Result<IReadOnlyList<Record>, Error>> FindRecordsAsync(
        UserId userId, DateTime start, 
        CancellationToken cancellationToken)
    {
        var list = new List<Record>();

        var end = start.AddDays(1);
        var period = new DateTimePeriod(start, end);
        var recordId = (RecordId?)null;
        
        do
        {
            if (list.Count != 0)
                recordId = list.Last().Id;

            var filter = new RecordByPeriodFilter(userId, period, recordId);

            var result = await _repository.FindAsync(filter, cancellationToken);
            if (!result.Success)
                return result.Error;

            var records = result.Value.ToBlockingEnumerable(cancellationToken);
            
            var countBefore = list.Count;
            list.AddRange(records);

            if (list.Count == countBefore || list.Count >= result.Value.Count)
                return Result<IReadOnlyList<Record>, Error>.CreateSuccess(list);
        } while (true);
    }
}