using Freem.Entities.Storage.Abstractions.Models.Identifiers;
using Freem.Entities.Storage.Abstractions.Repositories;
using Freem.Entities.UseCases.Events.Abstractions;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.Records.Remove.Models;
using Freem.Locking.Abstractions;
using Freem.Locking.Abstractions.Extensions;
using Freem.Storage.Abstractions.Helpers;
using Freem.Storage.Abstractions.Helpers.Extensions;

namespace Freem.Entities.UseCases.Records.Remove;

internal sealed class RemoveRecordUseCase : IUseCase<RemoveRecordRequest>
{
    private readonly IDistributedLocker _locker;
    private readonly IRecordsRepository _repository;
    private readonly IEventProducer _eventProducer;
    private readonly StorageTransactionRunner _transactionRunner;

    public RemoveRecordUseCase(
        IDistributedLocker locker,
        IRecordsRepository repository, 
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
        UseCaseExecutionContext context, RemoveRecordRequest request,
        CancellationToken cancellationToken = default)
    {
        context.ThrowsIfUnauthorized();
        
        await using var @lock = await _locker.LockAsync(Lock.Prefix + request.Id, cancellationToken);
        
        var ids = new RecordAndUserIdentifiers(request.Id, context.UserId);
        var result = await _repository.FindByMultipleIdAsync(ids, cancellationToken);
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