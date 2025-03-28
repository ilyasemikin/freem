﻿using Freem.Entities.Events.Producer.Implementations;
using Freem.Entities.RunningRecords;
using Freem.Entities.Storage.Abstractions.Repositories;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Contracts.Records.Create;
using Freem.Entities.UseCases.Contracts.RunningRecords.Stop;
using Freem.Locking.Abstractions;
using Freem.Locking.Abstractions.Extensions;
using Freem.Storage.Abstractions.Helpers;
using Freem.Storage.Abstractions.Helpers.Extensions;
using Freem.Time.Models;
using Freem.UseCases.Abstractions;

namespace Freem.Entities.UseCases.RunningRecords;

internal sealed class StopRunningRecordUseCase 
    : IEntitiesUseCase<StopRunningRecordRequest, StopRunningRecordResponse, StopRunningRecordErrorCode>
{
    private readonly IDistributedLocker _locker;
    private readonly IRunningRecordRepository _repository;
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;
    private readonly EventProducer _eventProducer;
    private readonly StorageTransactionRunner _transactionRunner;

    public StopRunningRecordUseCase(
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

    public async Task<StopRunningRecordResponse> ExecuteAsync(
        UseCaseExecutionContext context, StopRunningRecordRequest request,
        CancellationToken cancellationToken = default)
    {
        context.ThrowsIfUnauthorized();
        
        await using var @lock = await _locker.LockAsync(Lock.Prefix + context.UserId, cancellationToken);
        
        var result = await _repository.FindByIdAsync(context.UserId, cancellationToken);
        if (!result.Founded)
            return StopRunningRecordResponse.CreateFailure(StopRunningRecordErrorCode.NothingToStop);
        
        var record = result.Entity;
        if (record.StartAt > request.EndAt)
            return StopRunningRecordResponse.CreateFailure(
                StopRunningRecordErrorCode.EndAtToEarly, 
                $"EndAt must be greater than \"{record.StartAt:O}\".");
        
        var createRecordRequest = BuildCreateRecordRequest(record, request.EndAt);

        await _transactionRunner.RunAsync(async () =>
        {
            await _executor.ExecuteAsync<CreateRecordRequest, CreateRecordResponse>(context, createRecordRequest, cancellationToken);
            await _repository.DeleteAsync(record.Id, cancellationToken);
            await _eventProducer.PublishAsync(eventId => record.BuildStoppedEvent(eventId), cancellationToken);
        }, cancellationToken);
        
        return StopRunningRecordResponse.CreateSuccess();
    }
    
    private static CreateRecordRequest BuildCreateRecordRequest(RunningRecord record, DateTimeOffset endAt)
    {
        var period = new DateTimePeriod(record.StartAt, endAt);

        return new CreateRecordRequest(period, record.Activities)
        {
            Name = record.Name,
            Description = record.Description,
            
            Tags = record.Tags,
        };
    }
}