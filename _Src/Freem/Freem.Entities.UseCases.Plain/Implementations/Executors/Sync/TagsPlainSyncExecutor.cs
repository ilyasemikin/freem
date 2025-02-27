using Freem.Entities.Tags;
using Freem.Entities.Tags.Identifiers;
using Freem.Entities.UseCases.Contracts.Tags.Create;
using Freem.Entities.UseCases.Contracts.Tags.List;
using Freem.Entities.UseCases.Contracts.Tags.Update;
using Freem.Entities.UseCases.Plain.Implementations.Executors.Async;
using Freem.Results;
using Freem.UseCases.Abstractions;

namespace Freem.Entities.UseCases.Plain.Implementations.Executors.Sync;

public sealed class TagsPlainSyncExecutor
{
    private readonly TagsPlainExecutor _executor;

    public TagsPlainSyncExecutor(IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(executor);
        
        _executor = new TagsPlainExecutor(executor);
    }

    public Tag Create(UseCaseExecutionContext context, CreateTagRequest request)
    {
        return _executor.CreateAsync(context, request)
            .GetAwaiter()
            .GetResult();
    }

    public Tag Create(UseCaseExecutionContext context)
    {
        return _executor.CreateAsync(context)
            .GetAwaiter()
            .GetResult();
    }

    public IEnumerable<Tag> CreateMany(UseCaseExecutionContext context, IEnumerable<CreateTagRequest> requests)
    {
        return _executor.CreateManyAsync(context, requests)
            .ToArrayAsync()
            .Preserve()
            .GetAwaiter()
            .GetResult();
    }

    public IEnumerable<Tag> CreateMany(UseCaseExecutionContext context, int count)
    {
        return _executor.CreateManyAsync(context, count)
            .ToArrayAsync()
            .Preserve()
            .GetAwaiter()
            .GetResult();
    }

    public Tag Update(UseCaseExecutionContext context, UpdateTagRequest request)
    {
        return _executor.UpdateAsync(context, request)
            .GetAwaiter()
            .GetResult();
    }

    public void Remove(UseCaseExecutionContext context, TagIdentifier id)
    {
        _executor.RemoveAsync(context, id)
            .GetAwaiter()
            .GetResult();
    }

    public Result<Tag> Get(UseCaseExecutionContext context, TagIdentifier id)
    {
        return _executor.GetAsync(context, id)
            .GetAwaiter()
            .GetResult();
    }

    public Tag RequiredGet(UseCaseExecutionContext context, TagIdentifier id)
    {
        return _executor.RequiredGetAsync(context, id)
            .GetAwaiter()
            .GetResult();
    }

    public IEnumerable<Tag> List(UseCaseExecutionContext context, ListTagRequest request)
    {
        return _executor.ListAsync(context, request)
            .ToArrayAsync()
            .Preserve()
            .GetAwaiter()
            .GetResult();
    }
}