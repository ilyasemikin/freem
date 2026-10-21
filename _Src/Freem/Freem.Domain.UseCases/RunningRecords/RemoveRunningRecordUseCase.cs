using Freem.Domain.RunningRecords;
using Freem.Domain.Storage.Repositories.RunningRecords;
using Freem.Domain.UseCases.Events;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.RunningRecords.Remove;
using Freem.Storage.Transactions.Helpers;
using Freem.Storage.Transactions.Helpers.Extensions;
using Utilities.Errors;
using Utilities.Locking;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Domain.UseCases.RunningRecords;

internal sealed class RemoveRunningRecordUseCase 
    : IRequestHandler<HandlerContext, RemoveRunningRecordRequest, Result<Error>>
{
    private readonly ILocker _locker;
    private readonly IRunningRecordRepository _repository;
    private readonly EventProducer _eventProducer;
    private readonly StorageTransactionRunner _transactionRunner;

    public RemoveRunningRecordUseCase(
        ILocker locker, 
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
        var lockResult = await _locker.LockAsync(Lock.Prefix + context.UserId, cancellationToken);
        if (!lockResult.Success)
            return lockResult.Error;

        await using var @lock = lockResult.Value;

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

            return await _eventProducer.PublishAsync(record.UserId, eventId => record.BuildRemovedEvent(eventId), cancellationToken);
        }, cancellationToken);
    }
}