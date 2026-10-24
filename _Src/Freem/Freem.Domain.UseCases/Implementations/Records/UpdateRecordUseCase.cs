using Freem.Domain.Entities.Records;
using Freem.Domain.Storage.Models.Ids;
using Freem.Domain.Storage.Repositories.Records;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Records.Update;
using Freem.Domain.UseCases.Implementations.Events;
using Freem.Storage.Transactions.Implementations;
using Freem.Storage.Transactions.Implementations.Extensions;
using Utilities.Errors;
using Utilities.Locking;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Domain.UseCases.Implementations.Records;

internal sealed class UpdateRecordUseCase 
    : IRequestHandler<HandlerContext, UpdateRecordRequest, Result<Error>>
{
    private readonly ILocker _locker;
    private readonly IRecordRepository _repository;
    private readonly EventProducer _eventProducer;
    private readonly StorageTransactionRunner _transactionRunner;

    public UpdateRecordUseCase(
        ILocker locker,
        IRecordRepository repository, 
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
        HandlerContext context, UpdateRecordRequest request,
        CancellationToken cancellationToken = default)
    {
        if (!request.HasChanges())
            return DomainErrorCatalog.Factories.CreateNothingToDo();
        
        var lockResult = await _locker.LockAsync(Lock.Prefix + request.Id, cancellationToken);
        if (!lockResult.Success)
            return lockResult.Error;

        await using var @lock = lockResult.Value;
        
        var ids = new RecordAndUserIds(request.Id, context.UserId);
        var result = await _repository.FindByMultipleIdAsync(ids, cancellationToken);
        if (!result.Success)
            return result.Error;
        
        var record = result.Value;
        if (request.Name is not null)
            record.Name = request.Name;
        if (request.Description is not null)
            record.Description = request.Description;
        if (request.Activities is not null)
            record.Activities.Update(request.Activities);
        if (request.Tags is not null)
            record.Tags.Update(request.Tags);

        return await RunTransactionAsync(record, cancellationToken);
    }

    private async Task<Result<Error>> RunTransactionAsync(Record record, CancellationToken cancellationToken = default)
    {
        return await _transactionRunner.RunAsync(async () =>
        {
            var repositoryResult = await _repository.UpdateAsync(record, cancellationToken);
            if (!repositoryResult.Success)
                return repositoryResult;

            return await _eventProducer.PublishAsync(record.UserId, record.BuildUpdatedEvent, cancellationToken);
        }, cancellationToken);
    }
}