using Freem.Domain.Entities.Records;
using Freem.Domain.Entities.Statistics.Time;
using Freem.Domain.Entities.Users;
using Freem.Domain.Records;
using Freem.Domain.Storage.Repositories.Records;
using Freem.Domain.Users;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Contracts.Statistics.PerPeriod;
using Freem.Entities.UseCases.Contracts.Users.Settings.Get;
using Freem.Time.Extensions;
using Freem.Time.Models;
using Freem.UseCases.Abstractions;

namespace Freem.Entities.UseCases.Statistics;

internal sealed class StatisticsPerPeriodUseCase : 
    IEntitiesUseCase<StatisticsPerPeriodRequest, StatisticsPerPeriodResponse, StatisticsPerPeriodErrorCode>
{
    private readonly IRecordRepository _repository;
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;

    public StatisticsPerPeriodUseCase(
        IRecordRepository repository, 
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
        
        var time = TimeOnly.MinValue.Add(settings.DayUtcOffset.Value);
        var start = datePeriod.StartAt.ToUtcDateTime(time);
        var end = datePeriod.EndAt.ToUtcDateTime(time);
        
        var period = new DateTimePeriod(start, end);
        var records = await FindRecordsAsync(context.UserId, period, cancellationToken);

        var statistics = TimeStatisticsCalculator.Calculate(period, records);

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
            var records = result.ToBlockingEnumerable(cancellationToken);
            
            list.AddRange(records);

            if (list.Count >= result.TotalCount)
                return list;
        } while (true);
    }
}