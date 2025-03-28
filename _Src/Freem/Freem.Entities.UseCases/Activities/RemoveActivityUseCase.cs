﻿using Freem.Entities.Events.Producer.Implementations;
using Freem.Entities.Storage.Abstractions.Models.Identifiers;
using Freem.Entities.Storage.Abstractions.Repositories;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Contracts.Activities.Remove;
using Freem.Locking.Abstractions;
using Freem.Locking.Abstractions.Extensions;
using Freem.Storage.Abstractions.Helpers;
using Freem.Storage.Abstractions.Helpers.Extensions;

namespace Freem.Entities.UseCases.Activities;

internal sealed class RemoveActivityUseCase 
    : IEntitiesUseCase<RemoveActivityRequest, RemoveActivityResponse, RemoveActivityErrorCode>
{
    private readonly IDistributedLocker _locker;
    private readonly IActivitiesRepository _repository;
    private readonly EventProducer _eventProducer;
    private readonly StorageTransactionRunner _transactionRunner;

    public RemoveActivityUseCase(
        IDistributedLocker locker,
        IActivitiesRepository repository, 
        EventProducer eventProducer, 
        StorageTransactionRunner transactionRunner)
    {
        ArgumentNullException.ThrowIfNull(locker);
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(eventProducer);
        ArgumentNullException.ThrowIfNull(transactionRunner);
        
        _locker = locker;
        _repository = repository;
        _eventProducer = eventProducer;
        _transactionRunner = transactionRunner;
    }

    public async Task<RemoveActivityResponse> ExecuteAsync(
        UseCaseExecutionContext context, RemoveActivityRequest request, 
        CancellationToken cancellationToken = default)
    {
        context.ThrowsIfUnauthorized();
        
        await using var @lock = await _locker.LockAsync(Lock.Prefix+ request.Id, cancellationToken);
        
        var ids = new ActivityAndUserIdentifiers(request.Id, context.UserId);
        var result = await _repository.FindByMultipleIdAsync(ids, cancellationToken);
        if (!result.Founded)
            return RemoveActivityResponse.CreateFailure(RemoveActivityErrorCode.ActivityNotFound);

        var activity = result.Entity;

        await _transactionRunner.RunAsync(async () =>
        {
            await _repository.DeleteAsync(activity.Id, cancellationToken);
            await _eventProducer.PublishAsync(eventId => activity.BuildRemovedEvent(eventId), cancellationToken);
        }, cancellationToken);
        
        return RemoveActivityResponse.CreateSuccess();
    }
}