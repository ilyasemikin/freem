using System.Runtime.CompilerServices;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Identifiers;
using Freem.Entities.Records;
using Freem.Entities.Relations.Collections;
using Freem.Entities.UseCases.Contracts.Records.Create;
using Freem.Entities.UseCases.Contracts.Records.Get;
using Freem.Entities.UseCases.Contracts.Records.List;
using Freem.Entities.UseCases.Contracts.Records.Remove;
using Freem.Entities.UseCases.Contracts.Records.Update;
using Freem.Entities.UseCases.Plain.Exceptions;
using Freem.Results;
using Freem.Time.Models;
using Freem.UseCases.Abstractions;

namespace Freem.Entities.UseCases.Plain.Implementations.Executors.Async;

public sealed class RecordsPlainExecutor
{
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;

    public RecordsPlainExecutor(IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(executor);
        
        _executor = executor;
    }

    public async Task<Record> CreateAsync(
        UseCaseExecutionContext context, CreateRecordRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(request);
        
        var response = await _executor.ExecuteAsync<CreateRecordRequest, CreateRecordResponse>(context, request, cancellationToken);
        if (!response.Success)
            throw new PlainRequestFailedException();

        return response.Record;
    }

    public async Task<Record> CreateAsync(
        UseCaseExecutionContext context, IEnumerable<ActivityIdentifier> activityIds,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);

        var now = DateTimeOffset.UtcNow;

        var period = new DateTimePeriod(now.AddDays(-1), now);
        var activities = new RelatedActivitiesCollection(activityIds);
        
        var request = new CreateRecordRequest(period, activities);
        return await CreateAsync(context, request, cancellationToken);
    }

    public async IAsyncEnumerable<Record> CreateManyAsync(
        UseCaseExecutionContext context, IEnumerable<CreateRecordRequest> requests,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        foreach (var request in requests)
            yield return await CreateAsync(context, request, cancellationToken);
    }

    public async IAsyncEnumerable<Record> CreateManyAsync(
        UseCaseExecutionContext context, IReadOnlyList<ActivityIdentifier> activityIds, int count,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        foreach (var _ in Enumerable.Range(0, count))
            yield return await CreateAsync(context, activityIds, cancellationToken);
    }

    public async Task<Record> UpdateAsync(
        UseCaseExecutionContext context, UpdateRecordRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(request);
        
        var response = await _executor.ExecuteAsync<UpdateRecordRequest, UpdateRecordResponse>(context, request, cancellationToken);
        if (!response.Success)
            throw new PlainRequestFailedException();

        var result = await GetAsync(context, request.Id, cancellationToken);
        if (!result.Success)
            throw new PlainRequestFailedException();
        
        return result.Value;
    }

    public async Task RemoveAsync(
        UseCaseExecutionContext context, RecordIdentifier id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(id);
        
        var request = new RemoveRecordRequest(id);
        var response = await _executor.ExecuteAsync<RemoveRecordRequest, RemoveRecordResponse>(context, request, cancellationToken);
        if (!response.Success)
            throw new PlainRequestFailedException();
    }

    public async Task<Result<Record>> GetAsync(
        UseCaseExecutionContext context, RecordIdentifier id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(id);
        
        var request = new GetRecordRequest(id);
        var response = await _executor.ExecuteAsync<GetRecordRequest, GetRecordResponse>(context, request, cancellationToken);
        return response.Success
            ? Result<Record>.CreateSuccess(response.Record)
            : Result<Record>.CreateFailure();
    }

    public async Task<Record> RequiredGetAsync(
        UseCaseExecutionContext context, RecordIdentifier id,
        CancellationToken cancellationToken = default)
    {
        var result = await GetAsync(context, id, cancellationToken);
        if (!result.Success)
            throw new PlainRequestFailedException();

        return result.Value;
    }

    public async IAsyncEnumerable<Record> ListAsync(
        UseCaseExecutionContext context, ListRecordRequest request,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(request);
        
        var response = await _executor.ExecuteAsync<ListRecordRequest, ListRecordResponse>(context, request, cancellationToken);
        if (!response.Success)
            throw new PlainRequestFailedException();
        
        foreach (var record in response.Records)
            yield return record;
    }
}