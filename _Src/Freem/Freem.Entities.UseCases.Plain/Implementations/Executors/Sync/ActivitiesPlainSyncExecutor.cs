using Freem.Entities.Activities;
using Freem.Entities.Identifiers;
using Freem.Entities.UseCases.Contracts.Activities.Create;
using Freem.Entities.UseCases.Contracts.Activities.List;
using Freem.Entities.UseCases.Contracts.Activities.Update;
using Freem.Entities.UseCases.Plain.Implementations.Executors.Async;
using Freem.Results;
using Freem.UseCases.Abstractions;

namespace Freem.Entities.UseCases.Plain.Implementations.Executors.Sync;

public sealed class ActivitiesPlainSyncExecutor
{
    private readonly ActivitiesPlainExecutor _executor;

    public ActivitiesPlainSyncExecutor(IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(executor);
        
        _executor = new ActivitiesPlainExecutor(executor);
    }

    public Activity Create(UseCaseExecutionContext context, CreateActivityRequest request)
    {
        return _executor.CreateAsync(context, request)
            .GetAwaiter()
            .GetResult();
    }

    public Activity Create(UseCaseExecutionContext context)
    {
        return _executor.CreateAsync(context)
            .GetAwaiter()
            .GetResult();
    }

    public IEnumerable<Activity> CreateMany(
        UseCaseExecutionContext context, IEnumerable<CreateActivityRequest> requests)
    {
        return _executor.CreateManyAsync(context, requests)
            .ToArrayAsync()
            .Preserve()
            .GetAwaiter()
            .GetResult();
    }

    public IEnumerable<Activity> CreateMany(UseCaseExecutionContext context, int count)
    {
        return _executor.CreateManyAsync(context, count)
            .ToArrayAsync()
            .Preserve()
            .GetAwaiter()
            .GetResult();
    }

    public Activity Update(UseCaseExecutionContext context, UpdateActivityRequest request)
    {
        return _executor.UpdateAsync(context, request)
            .GetAwaiter()
            .GetResult();
    }

    public void Remove(UseCaseExecutionContext context, ActivityIdentifier id)
    {
        _executor.RemoveAsync(context, id)
            .GetAwaiter()
            .GetResult();
    }

    public void Archive(UseCaseExecutionContext context, ActivityIdentifier id)
    {
        _executor.ArchiveAsync(context, id)
            .GetAwaiter()
            .GetResult();
    }

    public void Unarchive(UseCaseExecutionContext context, ActivityIdentifier id)
    {
        _executor.UnarchiveAsync(context, id)
            .GetAwaiter()
            .GetResult();
    }

    public Result<Activity> Get(UseCaseExecutionContext context, ActivityIdentifier id)
    {
        return _executor.GetAsync(context, id)
            .GetAwaiter()
            .GetResult();
    }

    public Activity RequiredGet(UseCaseExecutionContext context, ActivityIdentifier id)
    {
        return _executor.RequiredGetAsync(context, id)
            .GetAwaiter()
            .GetResult();
    }

    public IEnumerable<Activity> List(UseCaseExecutionContext context, ListActivityRequest request)
    {
        return _executor.ListAsync(context, request)
            .ToArrayAsync()
            .Preserve()
            .GetAwaiter()
            .GetResult();
    }
}