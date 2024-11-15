﻿using Freem.Entities.Storage.Abstractions.Repositories;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.Events.Abstractions;
using Freem.Entities.UseCases.RunningRecords.Remove.Models;
using Freem.Locking.Abstractions;
using Freem.Locking.Abstractions.Extensions;
using Freem.Storage.Abstractions.Helpers;
using Freem.Storage.Abstractions.Helpers.Extensions;

namespace Freem.Entities.UseCases.RunningRecords.Remove;

internal sealed class RemoveRunningRecordUseCase : IUseCase<RemoveRunningRecordRequest>
{
    private readonly IDistributedLocker _locker;
    private readonly IRunningRecordRepository _repository;
    private readonly IEventProducer _eventProducer;
    private readonly StorageTransactionRunner _transactionRunner;

    public RemoveRunningRecordUseCase(
        IDistributedLocker locker, 
        IRunningRecordRepository repository, 
        IEventProducer eventProducer, 
        StorageTransactionRunner transactionRunner)
    {
        _locker = locker;
        _repository = repository;
        _eventProducer = eventProducer;
        _transactionRunner = transactionRunner;
    }

    public async Task ExecuteAsync(
        UseCaseExecutionContext context, RemoveRunningRecordRequest request,
        CancellationToken cancellationToken = default)
    {
        context.ThrowsIfUnauthorized();
        
        await using var @lock = await _locker.LockAsync(Lock.Prefix + context.UserId, cancellationToken);

        var result = await _repository.FindByIdAsync(context.UserId, cancellationToken);
        if (!result.Founded)
            throw new Exception();

        var record = result.Entity;
        
        await _transactionRunner.RunAsync(async () =>
        {
            await _repository.DeleteAsync(record.Id, cancellationToken);
            await _eventProducer.PublishAsync(eventId => record.BuildRemovedEvent(eventId), cancellationToken);
        }, cancellationToken);
    }
}