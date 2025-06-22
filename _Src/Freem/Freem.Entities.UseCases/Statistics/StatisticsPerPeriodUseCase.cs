using Freem.Entities.Records;
using Freem.Entities.Records.Identifiers;
using Freem.Entities.Statistics.Time;
using Freem.Entities.Storage.Abstractions.Base.Search;
using Freem.Entities.Storage.Abstractions.Models.Filters;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Contracts.Statistics.PerPeriod;
using Freem.Entities.UseCases.Contracts.Users.Settings.Get;
using Freem.Entities.Users;
using Freem.Entities.Users.Identifiers;
using Freem.Time.Extensions;
using Freem.Time.Models;
using Freem.UseCases.Abstractions;

namespace Freem.Entities.UseCases.Statistics;

internal sealed class StatisticsPerPeriodUseCase : 
    IEntitiesUseCase<StatisticsPerPeriodRequest, StatisticsPerPeriodResponse, StatisticsPerPeriodErrorCode>
{
    private readonly IMultipleSearchByFilterRepository<Record, RecordIdentifier, RecordsByPeriodFilter> _repository;
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;

    public StatisticsPerPeriodUseCase(
        IMultipleSearchByFilterRepository<Record, RecordIdentifier, RecordsByPeriodFilter> repository, 
        IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(executor);
        
        _repository = repository;
        _executor = executor;
    }

    public async Task<StatisticsPerPeriodResponse> ExecuteAsync(
        UseCaseExecutionContext context, 
        StatisticsPerPeriodRequest request,
        CancellationToken cancellationToken = default)
    {
        context.ThrowsIfUnauthorized();
        
        var settings = await GetUserSettingsAsync(context, cancellationToken);
        if (settings is null)
            return StatisticsPerPeriodResponse.CreateUsersNotFoundResult();

        var datePeriod = request.Period.ToDatePeriod();
        
        var time = TimeOnly.MinValue.Add(settings.DayUtcOffset);
        var start = datePeriod.StartAt.ToUtcDateTime(time);
        var end = datePeriod.EndAt.ToUtcDateTime(time);
        
        var period = new DateTimePeriod(start, end);
        var records = await FindRecordsAsync(context.UserId, period, cancellationToken);

        var statistics = TimeStatistics.Calculate(period, records);

        return StatisticsPerPeriodResponse.CreateSuccess(statistics);
    }
    
    private async Task<UserSettings?> GetUserSettingsAsync(
        UseCaseExecutionContext context, CancellationToken cancellationToken)
    {
        var request = new GetUserSettingsRequest();
        var response = await _executor.ExecuteAsync<GetUserSettingsRequest, GetUserSettingsResponse>(context, request, cancellationToken);
        return response.Settings;
    }

    private async Task<IReadOnlyList<Record>> FindRecordsAsync(
        UserIdentifier userId, DateTimePeriod period, 
        CancellationToken cancellationToken)
    {
        var list = new List<Record>();

        var start = period.StartAt;
        var end = period.EndAt;
        var recordId = (RecordIdentifier?)null;
        
        do
        {
            if (list.Count != 0)
            {
                var last = list.Last();
                start =  last.Period.StartAt.DateTime;
                recordId = last.Id;
            }
            
            period = new DateTimePeriod(start, end);
            var filter = new RecordsByPeriodFilter(userId, period, recordId);

            var result = await _repository.FindAsync(filter, cancellationToken);
            var records = result.ToBlockingEnumerable(cancellationToken);
            
            list.AddRange(records);

            if (list.Count >= result.TotalCount)
                return list;
        } while (true);
    }
}