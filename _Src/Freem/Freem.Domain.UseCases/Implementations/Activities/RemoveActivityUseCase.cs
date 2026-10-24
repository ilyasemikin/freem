using Freem.Domain.Entities.Activities;
using Freem.Domain.Storage.Models.Ids;
using Freem.Domain.Storage.Repositories.Activities;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Activities.Remove;
using Freem.Domain.UseCases.Implementations.Events;
using Freem.Storage.Transactions.Implementations;
using Freem.Storage.Transactions.Implementations.Extensions;
using Utilities.Errors;
using Utilities.Locking;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Domain.UseCases.Implementations.Activities;

internal sealed class RemoveActivityUseCase 
    : IRequestHandler<HandlerContext, RemoveActivityRequest, Result<Error>>
{
    private readonly ILocker _locker;
    private readonly IActivityRepository _repository;
    private readonly EventProducer _eventProducer;
    private readonly StorageTransactionRunner _transactionRunner;

    public RemoveActivityUseCase(
        ILocker locker,
        IActivityRepository repository, 
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

    public async Task<Result<Error>> HandleAsync(
        HandlerContext context, RemoveActivityRequest request, 
        CancellationToken cancellationToken = default)
    {
        var lockResult = await _locker.LockAsync(Lock.Prefix+ request.Id, cancellationToken);
        if (!lockResult.Success)
            return lockResult.Error;

        await using var @lock = lockResult.Value;
        
        var ids = new ActivityAndUserIds(request.Id, context.UserId);
        var result = await _repository.FindByMultipleIdAsync(ids, cancellationToken);
        if (!result.Success)
            return result.Error;

        var activity = result.Value;

        return await RunTransactionAsync(activity, cancellationToken);
    }

    private async Task<Result<Error>> RunTransactionAsync(Activity activity, CancellationToken cancellationToken = default)
    {
        return await _transactionRunner.RunAsync(async () =>
        {
            var result = await _repository.DeleteAsync(activity.Id, cancellationToken);
            if (!result.Success)
                return result;

            return await _eventProducer.PublishAsync(activity.UserId, eventId => activity.BuildRemovedEvent(eventId), cancellationToken);
        }, cancellationToken);
    }
}