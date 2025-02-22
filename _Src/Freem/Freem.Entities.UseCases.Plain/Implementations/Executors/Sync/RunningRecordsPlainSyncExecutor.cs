using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Identifiers;
using Freem.Entities.RunningRecords;
using Freem.Entities.UseCases.Contracts.RunningRecords.Start;
using Freem.Entities.UseCases.Contracts.RunningRecords.Update;
using Freem.Entities.UseCases.Plain.Implementations.Executors.Async;
using Freem.Results;
using Freem.UseCases.Abstractions;

namespace Freem.Entities.UseCases.Plain.Implementations.Executors.Sync;

public sealed class RunningRecordsPlainSyncExecutor
{
    private readonly RunningRecordsPlainExecutor _executors;

    public RunningRecordsPlainSyncExecutor(IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(executor);
        
        _executors = new RunningRecordsPlainExecutor(executor);
    }

    public RunningRecord Start(UseCaseExecutionContext context, StartRunningRecordRequest request)
    {
        return _executors.StartAsync(context, request)
            .GetAwaiter()
            .GetResult();
    }

    public RunningRecord Start(UseCaseExecutionContext context, IEnumerable<ActivityIdentifier> activityIds)
    {
        return _executors.StartAsync(context, activityIds)
            .GetAwaiter()
            .GetResult();
    }

    public void Stop(UseCaseExecutionContext context, DateTimeOffset endAt)
    {
        _executors.StopAsync(context, endAt)
            .GetAwaiter()
            .GetResult();
    }

    public void Stop(UseCaseExecutionContext context)
    {
        _executors.StopAsync(context)
            .GetAwaiter()
            .GetResult();
    }

    public RunningRecord Update(UseCaseExecutionContext context, UpdateRunningRecordRequest request)
    {
        return _executors.UpdateAsync(context, request)
            .GetAwaiter()
            .GetResult();
    }

    public void Remove(UseCaseExecutionContext context)
    {
        _executors.RemoveAsync(context)
            .GetAwaiter()
            .GetResult();
    }

    public Result<RunningRecord> Get(UseCaseExecutionContext context)
    {
        return _executors.GetAsync(context)
            .GetAwaiter()
            .GetResult();
    }

    public RunningRecord RequiredGet(UseCaseExecutionContext context)
    {
        return _executors.RequiredGetAsync(context)
            .GetAwaiter()
            .GetResult();
    }
}