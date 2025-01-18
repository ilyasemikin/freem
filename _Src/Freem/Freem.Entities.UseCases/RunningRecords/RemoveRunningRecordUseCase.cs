using Freem.Entities.Bus.Events.Abstractions;
using Freem.Entities.Storage.Abstractions.Repositories;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Contracts.RunningRecords.Remove;
using Freem.Locking.Abstractions;
using Freem.Locking.Abstractions.Extensions;
using Freem.Storage.Abstractions.Helpers;
using Freem.Storage.Abstractions.Helpers.Extensions;
using Freem.UseCases.Abstractions;

namespace Freem.Entities.UseCases.RunningRecords;

internal sealed class RemoveRunningRecordUseCase 
    : IEntitiesUseCase<RemoveRunningRecordRequest, RemoveRunningRecordResponse, RemoveRunningRecordErrorCode>
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
        ArgumentNullException.ThrowIfNull(locker);
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(eventProducer);
        ArgumentNullException.ThrowIfNull(transactionRunner);
        
        _locker = locker;
        _repository = repository;
        _eventProducer = eventProducer;
        _transactionRunner = transactionRunner;
    }

    public async Task<RemoveRunningRecordResponse> ExecuteAsync(
        UseCaseExecutionContext context, RemoveRunningRecordRequest request,
        CancellationToken cancellationToken = default)
    {
        context.ThrowsIfUnauthorized();
        
        await using var @lock = await _locker.LockAsync(Lock.Prefix + context.UserId, cancellationToken);

        var result = await _repository.FindByIdAsync(context.UserId, cancellationToken);
        if (!result.Founded)
            return RemoveRunningRecordResponse.CreateFailure(RemoveRunningRecordErrorCode.RunningRecordNotFound);

        var record = result.Entity;
        
        await _transactionRunner.RunAsync(async () =>
        {
            await _repository.DeleteAsync(record.Id, cancellationToken);
            await _eventProducer.PublishAsync(eventId => record.BuildRemovedEvent(eventId), cancellationToken);
        }, cancellationToken);

        return RemoveRunningRecordResponse.CreateSuccess();
    }
}