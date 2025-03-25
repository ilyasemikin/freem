using Freem.Entities.Activities.Identifiers.Extensions;
using Freem.Entities.Events.Producer.Implementations;
using Freem.Entities.RunningRecords;
using Freem.Entities.Storage.Abstractions.Exceptions;
using Freem.Entities.Storage.Abstractions.Repositories;
using Freem.Entities.Tags.Identifiers.Extensions;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Contracts.RunningRecords.Start;
using Freem.Entities.UseCases.Contracts.RunningRecords.Stop;
using Freem.Locking.Abstractions;
using Freem.Locking.Abstractions.Extensions;
using Freem.Storage.Abstractions.Helpers;
using Freem.Storage.Abstractions.Helpers.Extensions;
using Freem.UseCases.Abstractions;

namespace Freem.Entities.UseCases.RunningRecords;

internal sealed class StartRunningRecordUseCase 
    : IEntitiesUseCase<StartRunningRecordRequest, StartRunningRecordResponse, StartRunningRecordErrorCode>
{
    private readonly IDistributedLocker _locker;
    private readonly IRunningRecordRepository _repository;
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;
    private readonly EventProducer _eventProducer;
    private readonly StorageTransactionRunner _transactionRunner;

    public StartRunningRecordUseCase(
        IDistributedLocker locker,
        IRunningRecordRepository repository, 
        IUseCaseExecutor<UseCaseExecutionContext> executor, 
        EventProducer eventProducer, 
        StorageTransactionRunner transactionRunner)
    {
        ArgumentNullException.ThrowIfNull(locker);
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(executor);
        ArgumentNullException.ThrowIfNull(eventProducer);
        ArgumentNullException.ThrowIfNull(transactionRunner);

        _locker = locker;
        _repository = repository;
        _executor = executor;
        _eventProducer = eventProducer;
        _transactionRunner = transactionRunner;
    }

    public async Task<StartRunningRecordResponse> ExecuteAsync(
        UseCaseExecutionContext context, StartRunningRecordRequest request,
        CancellationToken cancellationToken = default)
    {
        context.ThrowsIfUnauthorized();
        
        var record = new RunningRecord(context.UserId, request.Activities, request.Tags, request.StartAt)
        {
            Name = request.Name,
            Description = request.Description
        };
        
        var currentResult = await _repository.FindByIdAsync(context.UserId, cancellationToken);
        if (currentResult.Founded)
        {
            var stopRunningRecordRequest = new StopRunningRecordRequest(record.StartAt);
            await _executor.ExecuteAsync<StopRunningRecordRequest, StopRunningRecordResponse>(context, stopRunningRecordRequest, cancellationToken);
        }

        await using var @lock = await _locker.LockAsync(Lock.Prefix + context.UserId, cancellationToken);
        try
        {
            await RunTransactionAsync(context, record, cancellationToken);
        }
        catch (NotFoundRelatedException ex)
        {
            return ProcessNotFoundRelatedException(ex);
        }
        
        return StartRunningRecordResponse.CreateSuccess(record);
    }

    private async Task RunTransactionAsync(
        UseCaseExecutionContext context, RunningRecord record, 
        CancellationToken cancellationToken = default)
    {
        await _transactionRunner.RunAsync(async () =>
        {
            await _repository.CreateAsync(record, cancellationToken);
            await _eventProducer.PublishAsync(record.BuildStartedEvent, cancellationToken);
        }, cancellationToken);
    }
    
    private static StartRunningRecordResponse ProcessNotFoundRelatedException(NotFoundRelatedException ex)
    {
        if (ex.RelatedIds.HasActivitiesIdentifiers())
            return StartRunningRecordResponse.CreateFailure(StartRunningRecordErrorCode.RelatedActivitiesNotFound);
        if (ex.RelatedIds.HasTagsIdentifiers())
            return StartRunningRecordResponse.CreateFailure(StartRunningRecordErrorCode.RelatedTagsNotFound);
        
        return StartRunningRecordResponse.CreateFailure(StartRunningRecordErrorCode.RelatedUnknownNotFound);
    }
}