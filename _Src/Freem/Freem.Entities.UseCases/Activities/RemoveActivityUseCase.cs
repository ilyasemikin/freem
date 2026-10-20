using Freem.Domain.Entities.Activities;
using Freem.Domain.Storage.Models.Ids;
using Freem.Domain.Storage.Repositories.Activities;
using Freem.Entities.Events.Producer.Implementations;
using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts.Activities.Remove;
using Freem.Locking.Abstractions;
using Freem.Locking.Abstractions.Extensions;
using Freem.Storage.Abstractions.Helpers;
using Freem.Storage.Abstractions.Helpers.Extensions;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Entities.UseCases.Activities;

internal sealed class RemoveActivityUseCase 
    : IRequestHandler<HandlerContext, RemoveActivityRequest, Result<Error>>
{
    private readonly IDistributedLocker _locker;
    private readonly IActivityRepository _repository;
    private readonly EventProducer _eventProducer;
    private readonly StorageTransactionRunner _transactionRunner;

    public RemoveActivityUseCase(
        IDistributedLocker locker,
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
        await using var @lock = await _locker.LockAsync(Lock.Prefix+ request.Id, cancellationToken);
        
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

            return await _eventProducer.PublishAsync(eventId => activity.BuildRemovedEvent(eventId), cancellationToken);
        }, cancellationToken);
    }
}