using Freem.Entities.Identifiers;
using Freem.Entities.Relations.Collections;
using Freem.Entities.RunningRecords;
using Freem.Entities.UseCases.Contracts.RunningRecords.Get;
using Freem.Entities.UseCases.Contracts.RunningRecords.Remove;
using Freem.Entities.UseCases.Contracts.RunningRecords.Start;
using Freem.Entities.UseCases.Contracts.RunningRecords.Stop;
using Freem.Entities.UseCases.Contracts.RunningRecords.Update;
using Freem.Entities.UseCases.Plain.Exceptions;
using Freem.Results;
using Freem.UseCases.Abstractions;

namespace Freem.Entities.UseCases.Plain.Implementations.Executors.Async;

public class RunningRecordsPlainExecutor
{
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;

    public RunningRecordsPlainExecutor(IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(executor);
        
        _executor = executor;
    }

    public async Task<RunningRecord> StartAsync(
        UseCaseExecutionContext context, StartRunningRecordRequest request, 
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(request);
        
        var response = await _executor.ExecuteAsync<StartRunningRecordRequest, StartRunningRecordResponse>(context, request, cancellationToken);
        if (!response.Success)
            throw new PlainRequestFailedException();

        return response.RunningRecord;
    }

    public async Task<RunningRecord> StartAsync(
        UseCaseExecutionContext context, IEnumerable<ActivityIdentifier> activityIds,
        CancellationToken cancellationToken = default)
    {
        var now = DateTimeOffset.UtcNow.AddDays(-1);
        var activities = new RelatedActivitiesCollection(activityIds);
        
        var request = new StartRunningRecordRequest(now, activities);
        return await StartAsync(context, request, cancellationToken);
    }

    public async Task StopAsync(
        UseCaseExecutionContext context, DateTimeOffset endAt, 
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        
        var request = new StopRunningRecordRequest(endAt);
        var response = await _executor.ExecuteAsync<StopRunningRecordRequest, StopRunningRecordResponse>(context, request, cancellationToken);
        if (!response.Success)
            throw new PlainRequestFailedException();
    }

    public async Task StopAsync(UseCaseExecutionContext context, CancellationToken cancellationToken = default)
    {
        var now = DateTimeOffset.UtcNow;
        
        await StopAsync(context, now, cancellationToken);
    }

    public async Task<RunningRecord> UpdateAsync(
        UseCaseExecutionContext context, UpdateRunningRecordRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(request);
        
        var response = await _executor.ExecuteAsync<UpdateRunningRecordRequest, UpdateRunningRecordResponse>(context, request, cancellationToken);
        if (!response.Success)
            throw new PlainRequestFailedException();
        
        var result = await GetAsync(context, cancellationToken);
        if (!result.Success)
            throw new PlainRequestFailedException();

        return result.Value;
    }

    public async Task RemoveAsync(
        UseCaseExecutionContext context, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        
        var request = RemoveRunningRecordRequest.Instance;
        var response = await _executor.ExecuteAsync<RemoveRunningRecordRequest, RemoveRunningRecordResponse>(context, request, cancellationToken);
        if (!response.Success)
            throw new PlainRequestFailedException();
    }

    public async Task<Result<RunningRecord>> GetAsync(
        UseCaseExecutionContext context, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        
        var request = GetRunningRecordRequest.Instance;
        var response = await _executor.ExecuteAsync<GetRunningRecordRequest, GetRunningRecordResponse>(context, request, cancellationToken);
        return response.Success
            ? Result<RunningRecord>.CreateSuccess(response.Record)
            : Result<RunningRecord>.CreateFailure();
    }

    public async Task<RunningRecord> RequiredGetAsync(
        UseCaseExecutionContext context, CancellationToken cancellationToken = default)
    {
        var result = await GetAsync(context, cancellationToken);
        if (!result.Success)
            throw new PlainRequestFailedException();
        
        return result.Value;
    }
}