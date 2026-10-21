using Freem.Domain;
using Freem.Domain.Entities.Activities;
using Freem.Domain.Storage.Models.Ids;
using Freem.Domain.Storage.Repositories.Activities;
using Freem.Domain.UseCases.Events;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Activities.Update;
using Freem.Locking;
using Freem.Locking.Extensions;
using Freem.Storage.Transactions.Helpers;
using Freem.Storage.Transactions.Helpers.Extensions;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Domain.UseCases.Activities;

internal sealed class UpdateActivityUseCase 
    : IRequestHandler<HandlerContext, UpdateActivityRequest, Result<Error>>
{
    private readonly IDistributedLocker _locker;
    private readonly IActivityRepository _repository;
    private readonly EventProducer _eventProducer;
    private readonly StorageTransactionRunner _transactionRunner;

    public UpdateActivityUseCase(
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
        HandlerContext context, UpdateActivityRequest request,
        CancellationToken cancellationToken = default)
    {
        if (!request.HasChanges())
            return DomainErrorCatalog.Factories.CreateNothingToDo();
        
        await using var @lock = await _locker.LockAsync(Lock.Prefix + request.Id, cancellationToken);
        
        var ids = new ActivityAndUserIds(request.Id, context.UserId);
        var result = await _repository.FindByMultipleIdAsync(ids, cancellationToken);
        if (!result.Success)
            return result.Error;
        
        var activity = result.Value;
        if (request.Name is not null)
            activity.Name = request.Name;
        if (request.Color is not null)
            activity.Color = request.Color;
        if (request.Tags is not null)
            activity.Tags.Update(request.Tags);

        return await RunTransactionAsync(activity, cancellationToken);
    }

    private async Task<Result<Error>> RunTransactionAsync(Activity activity, CancellationToken cancellationToken = default)
    {
        return await _transactionRunner.RunAsync(async () =>
        {
            var result = await _repository.UpdateAsync(activity, cancellationToken);
            if (!result.Success)
                return result;

            return await _eventProducer.PublishAsync(activity.UserId, activity.BuildUpdatedEvent, cancellationToken);
        }, cancellationToken);
    }
}