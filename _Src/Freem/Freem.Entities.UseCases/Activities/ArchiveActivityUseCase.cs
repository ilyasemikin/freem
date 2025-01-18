using Freem.Entities.Activities.Models;
using Freem.Entities.Bus.Events.Abstractions;
using Freem.Entities.Storage.Abstractions.Models.Identifiers;
using Freem.Entities.Storage.Abstractions.Repositories;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Contracts.Activities.Archive;
using Freem.Locking.Abstractions;
using Freem.Locking.Abstractions.Extensions;
using Freem.Storage.Abstractions.Helpers;
using Freem.Storage.Abstractions.Helpers.Extensions;
using Freem.UseCases.Abstractions;

namespace Freem.Entities.UseCases.Activities;

internal class ArchiveActivityUseCase : 
    IEntitiesUseCase<ArchiveActivityRequest, ArchiveActivityResponse, ArchiveActivityErrorCode>
{
    private readonly IDistributedLocker _locker;
    private readonly IActivitiesRepository _repository;
    private readonly IEventProducer _eventProducer;
    private readonly StorageTransactionRunner _transactionRunner;

    public ArchiveActivityUseCase(
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

    public async Task<ArchiveActivityResponse> ExecuteAsync(
        UseCaseExecutionContext context, ArchiveActivityRequest request,
        CancellationToken cancellationToken = default)
    {
        context.ThrowsIfUnauthorized();
        
        await using var @lock = await _locker.LockAsync(Lock.Prefix + request.Id, cancellationToken);
        
        var ids = new ActivityAndUserIdentifiers(request.Id, context.UserId);
        var result = await _repository.FindByMultipleIdAsync(ids, cancellationToken);
        if (!result.Founded)
            return ArchiveActivityResponse.CreateFailure(ArchiveActivityErrorCode.ActivityNotFound);

        var activity = result.Entity;
        if (activity.Status == ActivityStatus.Archived)
            return ArchiveActivityResponse.CreateFailure(ArchiveActivityErrorCode.ActivityInvalidStatus);
        
        activity.Status = ActivityStatus.Value.Archived;

        await _transactionRunner.RunAsync(async () =>
        {
            await _repository.UpdateAsync(activity, cancellationToken);
            await _eventProducer.PublishAsync(activity.BuildArchivedEvent, cancellationToken);
        }, cancellationToken);

        return ArchiveActivityResponse.CreateSuccess();
    }
}