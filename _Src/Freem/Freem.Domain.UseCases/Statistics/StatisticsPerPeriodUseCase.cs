using Freem.Domain.Entities.Records;
using Freem.Domain.Entities.Statistics.Time;
using Freem.Domain.Records;
using Freem.Domain.Storage.Repositories.Records;
using Freem.Domain.Users;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Statistics.PerPeriod;
using Freem.Domain.UseCases.Users.Settings;
using Freem.Time.Models;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Domain.UseCases.Statistics;

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
        var settingsResult = await _mediator.GetUserSettingsAsync(context, cancellationToken);
        if (!settingsResult.Success)
            return settingsResult.Error;

        var settings = settingsResult.Value;

        var datePeriod = request.Period.ToDatePeriod();
        
        var start = settings.TimeZoneOffset.GetDayStartUtc(datePeriod.StartAt);
        var end = settings.TimeZoneOffset.GetDayStartUtc(datePeriod.EndAt);
        
        var period = new DateTimePeriod(start, end);
        var recordsResult = await FindRecordsAsync(context.UserId, period, cancellationToken);
        if (!recordsResult.Success)
            return recordsResult.Error;

        var records = recordsResult.Value;

        var statistics = TimeStatisticsCalculator.Calculate(period, records);

        return new StatisticsPerPeriodResponse(statistics);
    }
    
    private async Task<Result<IReadOnlyList<Record>, Error>> FindRecordsAsync(
        UserId userId, DateTimePeriod period, 
        CancellationToken cancellationToken)
    {
        var list = new List<Record>();

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