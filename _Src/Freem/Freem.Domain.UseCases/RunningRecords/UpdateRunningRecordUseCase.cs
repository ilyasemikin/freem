using Freem.Domain;
using Freem.Domain.Entities.RunningRecords;
using Freem.Domain.RunningRecords;
using Freem.Domain.Storage.Repositories.RunningRecords;
using Freem.Domain.UseCases.Events;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.RunningRecords.Update;
using Freem.Storage.Transactions.Helpers;
using Freem.Storage.Transactions.Helpers.Extensions;
using Utilities.Errors;
using Utilities.Locking;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Domain.UseCases.RunningRecords;

internal class UpdateRunningRecordUseCase 
    : IRequestHandler<HandlerContext, UpdateRunningRecordRequest, Result<Error>>
{
    private readonly ILocker _locker;
    private readonly IRunningRecordRepository _repository;
    private readonly EventProducer _eventProducer;
    private readonly StorageTransactionRunner _transactionRunner;

    public UpdateRunningRecordUseCase(
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
        HandlerContext context, UpdateRunningRecordRequest request,
        CancellationToken cancellationToken = default)
    {
        if (!request.HasChanges())
            return DomainErrorCatalog.Factories.CreateNothingToDo();

        var lockResult = await _locker.LockAsync(Lock.Prefix + context.UserId, cancellationToken);
        if (!lockResult.Success)
            return lockResult.Error;

        await using var @lock = lockResult.Value;
        
        var id = RunningRecordId.FromUserId(context.UserId);
        var result = await _repository.FindByIdAsync(id, cancellationToken);
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
    
    private async Task<Result<Error>> RunTransactionAsync(RunningRecord record, CancellationToken cancellationToken = default)
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