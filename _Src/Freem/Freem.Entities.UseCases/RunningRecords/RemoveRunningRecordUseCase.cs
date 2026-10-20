using Freem.Domain.RunningRecords;
using Freem.Domain.Storage.Repositories.RunningRecords;
using Freem.Entities.Events.Producer.Implementations;
using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts.RunningRecords.Remove;
using Freem.Locking.Abstractions;
using Freem.Locking.Abstractions.Extensions;
using Freem.Storage.Abstractions.Helpers;
using Freem.Storage.Abstractions.Helpers.Extensions;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Entities.UseCases.RunningRecords;

internal sealed class RemoveRunningRecordUseCase 
    : IRequestHandler<HandlerContext, RemoveRunningRecordRequest, Result<Error>>
{
    private readonly IDistributedLocker _locker;
    private readonly IRunningRecordRepository _repository;
    private readonly EventProducer _eventProducer;
    private readonly StorageTransactionRunner _transactionRunner;

    public RemoveRunningRecordUseCase(
        IDistributedLocker locker, 
        IRunningRecordRepository repository, 
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
        HandlerContext context, RemoveRunningRecordRequest request,
        CancellationToken cancellationToken = default)
    {
        await using var @lock = await _locker.LockAsync(Lock.Prefix + context.UserId, cancellationToken);

        var id = RunningRecordId.FromUserId(context.UserId);
        var result = await _repository.FindByIdAsync(id, cancellationToken);
        if (!result.Success)
            return result.Error;

        var record = result.Value;
        
        return await _transactionRunner.RunAsync(async () =>
        {
            var repositoryResult = await _repository.DeleteAsync(record.Id, cancellationToken);
            if (!repositoryResult.Success)
                return repositoryResult;

            return await _eventProducer.PublishAsync(eventId => record.BuildRemovedEvent(eventId), cancellationToken);
        }, cancellationToken);
    }
}