using Freem.Entities.Activities.Models;
using Freem.Entities.Storage.Abstractions.Models.Identifiers;
using Freem.Entities.Storage.Abstractions.Repositories;
using Freem.Entities.UseCases.Events.Abstractions;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.Activities.Unarchive.Models;
using Freem.Locking.Abstractions;
using Freem.Locking.Abstractions.Extensions;
using Freem.Storage.Abstractions.Helpers;
using Freem.Storage.Abstractions.Helpers.Extensions;

namespace Freem.Entities.UseCases.Activities.Unarchive;

internal sealed class UnarchiveActivityUseCase : IUseCase<UnarchiveActivityRequest>
{
    private readonly IDistributedLocker _locker;
    private readonly IActivitiesRepository _repository;
    private readonly IEventProducer _eventProducer;
    private readonly StorageTransactionRunner _transactionRunner;

    public UnarchiveActivityUseCase(
        IDistributedLocker locker,
        IActivitiesRepository repository, 
        IEventProducer eventProducer, 
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

    public async Task ExecuteAsync(
        UseCaseExecutionContext context, UnarchiveActivityRequest request,
        CancellationToken cancellationToken = default)
    {
        context.ThrowsIfUnauthorized();
        
        await using var @lock = await _locker.LockAsync(Lock.Prefix + request.Id, cancellationToken);
        
        var ids = new ActivityAndUserIdentifiers(request.Id, context.UserId);
        var result = await _repository.FindByMultipleIdAsync(ids, cancellationToken);
        if (!result.Founded)
            throw new Exception();
        
        var activity = result.Entity;
        activity.Status = ActivityStatus.Value.Active;

        await _transactionRunner.RunAsync(async () =>
        {
            await _repository.UpdateAsync(activity, cancellationToken);
            await _eventProducer.PublishAsync(eventId => activity.BuildUnarchivedEvent(eventId), cancellationToken);
        }, cancellationToken);
    }
}