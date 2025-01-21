using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Records;
using Freem.Entities.Records.Identifiers;
using Freem.Entities.UseCases.Contracts.Records.Create;
using Freem.Entities.UseCases.Contracts.Records.List;
using Freem.Entities.UseCases.Contracts.Records.Update;
using Freem.Entities.UseCases.Plain.Implementations.Executors.Async;
using Freem.Results;
using Freem.UseCases.Abstractions;

namespace Freem.Entities.UseCases.Plain.Implementations.Executors.Sync;

public sealed class RecordsPlainSyncExecutor
{
    private readonly RecordsPlainExecutor _executor;

    public RecordsPlainSyncExecutor(IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(executor);
        
        _executor = new RecordsPlainExecutor(executor);
    }

    public Record Create(UseCaseExecutionContext context, CreateRecordRequest request)
    {
        return _executor.CreateAsync(context, request)
            .GetAwaiter()
            .GetResult();
    }

    public Record Create(UseCaseExecutionContext context, IEnumerable<ActivityIdentifier> activityIds)
    {
        return _executor.CreateAsync(context, activityIds)
            .GetAwaiter()
            .GetResult();
    }

    public IEnumerable<Record> CreateMany(UseCaseExecutionContext context, IEnumerable<CreateRecordRequest> requests)
    {
        return _executor.CreateManyAsync(context, requests)
            .ToArrayAsync()
            .Preserve()
            .GetAwaiter()
            .GetResult();
    }

    public IEnumerable<Record> CreateMany(
        UseCaseExecutionContext context, IReadOnlyList<ActivityIdentifier> activityIds, int count)
    {
        return _executor.CreateManyAsync(context, activityIds, count)
            .ToArrayAsync()
            .Preserve()
            .GetAwaiter()
            .GetResult();
    }

    public Record Update(UseCaseExecutionContext context, UpdateRecordRequest request)
    {
        return _executor.UpdateAsync(context, request)
            .GetAwaiter()
            .GetResult();
    }

    public void Remove(UseCaseExecutionContext context, RecordIdentifier id)
    {
        _executor.RemoveAsync(context, id)
            .GetAwaiter()
            .GetResult();
    }

    public Result<Record> Get(UseCaseExecutionContext context, RecordIdentifier id)
    {
        return _executor.GetAsync(context, id)
            .GetAwaiter()
            .GetResult();
    }

    public Record RequiredGet(UseCaseExecutionContext context, RecordIdentifier id)
    {
        return _executor.RequiredGetAsync(context, id)
            .GetAwaiter()
            .GetResult();
    }

    public IEnumerable<Record> List(UseCaseExecutionContext context, ListRecordRequest request)
    {
        return _executor.ListAsync(context, request)
            .ToArrayAsync()
            .Preserve()
            .GetAwaiter()
            .GetResult();
    }
}