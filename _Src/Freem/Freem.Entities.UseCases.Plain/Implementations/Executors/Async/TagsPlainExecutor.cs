using System.Runtime.CompilerServices;
using Freem.Entities.Identifiers;
using Freem.Entities.Tags;
using Freem.Entities.Tags.Identifiers;
using Freem.Entities.UseCases.Contracts.Tags.Create;
using Freem.Entities.UseCases.Contracts.Tags.Get;
using Freem.Entities.UseCases.Contracts.Tags.List;
using Freem.Entities.UseCases.Contracts.Tags.Remove;
using Freem.Entities.UseCases.Contracts.Tags.Update;
using Freem.Entities.UseCases.Plain.Exceptions;
using Freem.Results;
using Freem.UseCases.Abstractions;

namespace Freem.Entities.UseCases.Plain.Implementations.Executors.Async;

public sealed class TagsPlainExecutor
{
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;

    public TagsPlainExecutor(IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(executor);
        
        _executor = executor;
    }

    public async Task<Tag> CreateAsync(
        UseCaseExecutionContext context, CreateTagRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(request);
        
        var response = await _executor.ExecuteAsync<CreateTagRequest, CreateTagResponse>(context, request, cancellationToken);
        if (!response.Success)
            throw new PlainRequestFailedException();

        return response.Tag;
    }

    public async Task<Tag> CreateAsync(UseCaseExecutionContext context, CancellationToken cancellationToken = default)
    {
        var name = Guid.NewGuid().ToString();
        
        var request = new CreateTagRequest(name);
        return await CreateAsync(context, request, cancellationToken);
    }

    public async IAsyncEnumerable<Tag> CreateManyAsync(
        UseCaseExecutionContext context, IEnumerable<CreateTagRequest> requests,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(requests);
        
        foreach (var request in requests)
            yield return await CreateAsync(context, request, cancellationToken);
    }

    public async IAsyncEnumerable<Tag> CreateManyAsync(
        UseCaseExecutionContext context, int count, 
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(count);

        foreach (var _ in Enumerable.Range(0, count))
            yield return await CreateAsync(context, cancellationToken);
    }

    public async Task<Tag> UpdateAsync(
        UseCaseExecutionContext context, UpdateTagRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(request);
        
        var response = await _executor.ExecuteAsync<UpdateTagRequest, UpdateTagResponse>(context, request, cancellationToken);
        if (!response.Success)
            throw new PlainRequestFailedException();

        var result = await GetAsync(context, request.Id, cancellationToken);
        if (!result.Success)
            throw new PlainRequestFailedException();

        return result.Value;
    }

    public async Task RemoveAsync(
        UseCaseExecutionContext context, TagIdentifier id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(id);
        
        var request = new RemoveTagRequest(id);
        var response = await _executor.ExecuteAsync<RemoveTagRequest, RemoveTagResponse>(context, request, cancellationToken);
        if (!response.Success)
            throw new PlainRequestFailedException();
    }

    public async Task<Result<Tag>> GetAsync(
        UseCaseExecutionContext context, TagIdentifier id, 
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(id);
        
        var request = new GetTagRequest(id);
        var response = await _executor.ExecuteAsync<GetTagRequest, GetTagResponse>(context, request, cancellationToken);
        if (!response.Success)
            throw new PlainRequestFailedException();

        return response.Success
            ? Result<Tag>.CreateSuccess(response.Tag)
            : Result<Tag>.CreateFailure();
    }

    public async Task<Tag> RequiredGetAsync(
        UseCaseExecutionContext context, TagIdentifier id,
        CancellationToken cancellationToken = default)
    {
        var result = await GetAsync(context, id, cancellationToken);
        if (!result.Success)
            throw new PlainRequestFailedException();
        
        return result.Value;
    }

    public async IAsyncEnumerable<Tag> ListAsync(
        UseCaseExecutionContext context, ListTagRequest request,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(request);
        
        var response = await _executor.ExecuteAsync<ListTagRequest, ListTagResponse>(context, request, cancellationToken);
        if (!response.Success)
            throw new PlainRequestFailedException();
        
        foreach (var tag in response.Tags)
            yield return tag;
    }
}