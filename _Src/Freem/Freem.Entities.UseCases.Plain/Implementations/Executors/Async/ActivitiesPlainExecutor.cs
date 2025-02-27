using System.Runtime.CompilerServices;
using Freem.Entities.Activities;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.UseCases.Contracts.Activities.Archive;
using Freem.Entities.UseCases.Contracts.Activities.Create;
using Freem.Entities.UseCases.Contracts.Activities.Get;
using Freem.Entities.UseCases.Contracts.Activities.List;
using Freem.Entities.UseCases.Contracts.Activities.Remove;
using Freem.Entities.UseCases.Contracts.Activities.Unarchive;
using Freem.Entities.UseCases.Contracts.Activities.Update;
using Freem.Entities.UseCases.Plain.Exceptions;
using Freem.Results;
using Freem.UseCases.Abstractions;

namespace Freem.Entities.UseCases.Plain.Implementations.Executors.Async;

public sealed class ActivitiesPlainExecutor
{
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;

    public ActivitiesPlainExecutor(IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(executor);
        
        _executor = executor;
    }

    public async Task<Activity> CreateAsync(
        UseCaseExecutionContext context, CreateActivityRequest request, 
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(request);
        
        var response = await _executor.ExecuteAsync<CreateActivityRequest, CreateActivityResponse>(context, request, cancellationToken);

        if (!response.Success)
            throw new PlainRequestFailedException();

        return response.Activity;
    }

    public async Task<Activity> CreateAsync(
        UseCaseExecutionContext context, CancellationToken cancellationToken = default)
    {
        var name = Guid.NewGuid().ToString();
        
        var request = new CreateActivityRequest(name);
        return await CreateAsync(context, request, cancellationToken);
    }

    public async IAsyncEnumerable<Activity> CreateManyAsync(
        UseCaseExecutionContext context, IEnumerable<CreateActivityRequest> requests,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        foreach (var request in requests)
            yield return await CreateAsync(context, request, cancellationToken);
    }

    public async IAsyncEnumerable<Activity> CreateManyAsync(
        UseCaseExecutionContext context, int count,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        foreach (var _ in Enumerable.Range(0, count))
            yield return await CreateAsync(context, cancellationToken);
    }

    public async Task<Activity> UpdateAsync(
        UseCaseExecutionContext context, UpdateActivityRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(request);
        
        var response = await _executor.ExecuteAsync<UpdateActivityRequest, UpdateActivityResponse>(context, request, cancellationToken);
        if (!response.Success)
            throw new PlainRequestFailedException();

        var result = await GetAsync(context, request.Id, cancellationToken);
        if (!result.Success)
            throw new PlainRequestFailedException();

        return result.Value;
    }

    public async Task RemoveAsync(
        UseCaseExecutionContext context, ActivityIdentifier id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(id);
        
        var request = new RemoveActivityRequest(id);
        var response = await _executor.ExecuteAsync<RemoveActivityRequest, RemoveActivityResponse>(context, request, cancellationToken);
        if (!response.Success)
            throw new PlainRequestFailedException();
    }

    public async Task ArchiveAsync(
        UseCaseExecutionContext context, ActivityIdentifier id, 
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(id);
        
        var request = new ArchiveActivityRequest(id);
        var response = await _executor.ExecuteAsync<ArchiveActivityRequest, ArchiveActivityResponse>(context, request, cancellationToken);
        if (!response.Success)
            throw new PlainRequestFailedException();
    }

    public async Task UnarchiveAsync(
        UseCaseExecutionContext context, ActivityIdentifier id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(id);
        
        var request = new UnarchiveActivityRequest(id);
        var response = await _executor.ExecuteAsync<UnarchiveActivityRequest, UnarchiveActivityResponse>(context, request, cancellationToken);
        if (!response.Success)
            throw new PlainRequestFailedException();
    }

    public async Task<Result<Activity>> GetAsync(
        UseCaseExecutionContext context, ActivityIdentifier id, 
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(id);
        
        var request = new GetActivityRequest(id);
        var response = await _executor.ExecuteAsync<GetActivityRequest, GetActivityResponse>(context, request, cancellationToken);
        return response.Success
            ? Result<Activity>.CreateSuccess(response.Activity)
            : Result<Activity>.CreateFailure();
    }

    public async Task<Activity> RequiredGetAsync(
        UseCaseExecutionContext context, ActivityIdentifier id,
        CancellationToken cancellationToken = default)
    {
        var result = await GetAsync(context, id, cancellationToken);
        if (!result.Success)
            throw new PlainRequestFailedException();
        
        return result.Value;
    }

    public async IAsyncEnumerable<Activity> ListAsync(
        UseCaseExecutionContext context, ListActivityRequest request,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(request);
        
        var response = await _executor.ExecuteAsync<ListActivityRequest, ListActivityResponse>(context, request, cancellationToken);
        if (!response.Success)
            throw new PlainRequestFailedException();

        foreach (var activity in response.Activities)
            yield return activity;
    }
}