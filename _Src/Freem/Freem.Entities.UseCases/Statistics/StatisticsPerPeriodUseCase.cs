using Freem.Domain.Entities.Records;
using Freem.Domain.Entities.Statistics.Time;
using Freem.Domain.Entities.Users;
using Freem.Domain.Records;
using Freem.Domain.Storage.Repositories.Records;
using Freem.Domain.Users;
using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts.Statistics.PerPeriod;
using Freem.Entities.UseCases.Contracts.Users.Settings.Get;
using Freem.Time.Extensions;
using Freem.Time.Models;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Entities.UseCases.Statistics;

internal sealed class StatisticsPerPeriodUseCase : 
    IRequestHandler<HandlerContext, StatisticsPerPeriodRequest, Result<StatisticsPerPeriodResponse, Error>>
{
    private readonly IRecordRepository _repository;
    private readonly IRequestMediator<HandlerContext> _mediator;

    public StatisticsPerPeriodUseCase(
        IRecordRepository repository, 
        IRequestMediator<HandlerContext> mediator)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(mediator);
        
        _repository = repository;
        _mediator = mediator;
    }

    public async Task<Result<StatisticsPerPeriodResponse, Error>> HandleAsync(
        HandlerContext context, 
        StatisticsPerPeriodRequest request,
        CancellationToken cancellationToken = default)
    {
        var settingsResult = await GetUserSettingsAsync(context, cancellationToken);
        if (!settingsResult.Success)
            return settingsResult.Error;

        var settings = settingsResult.Value;

        var datePeriod = request.Period.ToDatePeriod();
        
        var time = TimeOnly.MinValue.Add(settings.DayUtcOffset.Value);
        var start = datePeriod.StartAt.ToUtcDateTime(time);
        var end = datePeriod.EndAt.ToUtcDateTime(time);
        
        var period = new DateTimePeriod(start, end);
        var recordsResult = await FindRecordsAsync(context.UserId, period, cancellationToken);
        if (!recordsResult.Success)
            return recordsResult.Error;

        var records = recordsResult.Value;

        var statistics = TimeStatisticsCalculator.Calculate(period, records);

        return new StatisticsPerPeriodResponse(statistics);
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
        UserId userId, DateTimePeriod period, 
        CancellationToken cancellationToken)
    {
        var list = new List<Record>();

        var start = period.StartAt;
        var end = period.EndAt;
        var recordId = (RecordId?)null;
        
        do
        {
            if (list.Count != 0)
            {
                var last = list.Last();
                start =  last.Period.StartAt.DateTime;
                recordId = last.Id;
            }
            
            period = new DateTimePeriod(start, end);
            var filter = new RecordByPeriodFilter(userId, period, recordId);

            var result = await _repository.FindAsync(filter, cancellationToken);
            if (!result.Success)
                return result.Error;

            var records = result.Value.ToBlockingEnumerable(cancellationToken);
            
            list.AddRange(records);

            if (list.Count >= result.Value.Count)
                return Result<IReadOnlyList<Record>, Error>.CreateSuccess(list);
        } while (true);
    }
}