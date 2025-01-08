using Freem.Entities.Activities.Identifiers.Extensions;
using Freem.Entities.RunningRecords;
using Freem.Entities.Storage.Abstractions.Exceptions;
using Freem.Entities.Storage.Abstractions.Repositories;
using Freem.Entities.Tags.Identifiers.Extensions;
using Freem.Entities.UseCases.Events.Abstractions;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.RunningRecords.Update.Models;
using Freem.Locking.Abstractions;
using Freem.Locking.Abstractions.Extensions;
using Freem.Storage.Abstractions.Helpers;
using Freem.Storage.Abstractions.Helpers.Extensions;

namespace Freem.Entities.UseCases.RunningRecords.Update;

internal class UpdateRunningRecordUseCase : IUseCase<UpdateRunningRecordRequest, UpdateRunningRecordResponse>
{
    private readonly IDistributedLocker _locker;
    private readonly IRunningRecordRepository _repository;
    private readonly IEventProducer _eventProducer;
    private readonly StorageTransactionRunner _transactionRunner;

    public UpdateRunningRecordUseCase(
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

    public async Task<UpdateRunningRecordResponse> ExecuteAsync(
        UseCaseExecutionContext context, UpdateRunningRecordRequest request,
        CancellationToken cancellationToken = default)
    {
        context.ThrowsIfUnauthorized();
        
        await using var @lock = await _locker.LockAsync(Lock.Prefix + context.UserId, cancellationToken);

        if (!request.HasChanges())
            return UpdateRunningRecordResponse.CreateFailure(UpdateRunningRecordErrorCode.NothingToUpdate);
        
        var result = await _repository.FindByIdAsync(context.UserId, cancellationToken);
        if (!result.Founded)
            return UpdateRunningRecordResponse.CreateFailure(UpdateRunningRecordErrorCode.RunningRecordNotFound);
        
        var record = result.Entity;
        if (request.Name is not null)
            record.Name = request.Name;
        if (request.Description is not null)
            record.Description = request.Description;
        if (request.Activities is not null)
            record.Activities.Update(request.Activities);
        if (request.Tags is not null)
            record.Tags.Update(request.Tags);

        try
        {
            await RunTransactionAsync(record, cancellationToken);
        }
        catch (NotFoundRelatedException ex)
        {
            return ProcessNotFoundRelatedException(ex);
        }

        return UpdateRunningRecordResponse.CreateSuccess();
    }
    
    private async Task RunTransactionAsync(RunningRecord record, CancellationToken cancellationToken = default)
    {
        await _transactionRunner.RunAsync(async () =>
        {
            await _repository.UpdateAsync(record, cancellationToken);
            await _eventProducer.PublishAsync(record.BuildUpdatedEvent, cancellationToken);
        }, cancellationToken);
    }

    private static UpdateRunningRecordResponse ProcessNotFoundRelatedException(NotFoundRelatedException ex)
    {
        if (ex.RelatedIds.HasActivitiesIdentifiers())
            return UpdateRunningRecordResponse.CreateFailure(UpdateRunningRecordErrorCode.RelatedActivitiesNotFound);
        if (ex.RelatedIds.HasTagsIdentifiers())
            return UpdateRunningRecordResponse.CreateFailure(UpdateRunningRecordErrorCode.RelatedTagsNotFound);
        
        return UpdateRunningRecordResponse.CreateFailure(UpdateRunningRecordErrorCode.RelatedUnknownNotFound);
    }
}