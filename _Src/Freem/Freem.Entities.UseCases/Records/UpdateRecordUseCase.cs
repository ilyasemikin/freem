using Freem.Entities.Activities.Identifiers.Extensions;
using Freem.Entities.Events.Production.Implementations;
using Freem.Entities.Records;
using Freem.Entities.Storage.Abstractions.Exceptions;
using Freem.Entities.Storage.Abstractions.Models.Identifiers;
using Freem.Entities.Storage.Abstractions.Repositories;
using Freem.Entities.Tags.Identifiers.Extensions;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Contracts.Records.Update;
using Freem.Locking.Abstractions;
using Freem.Locking.Abstractions.Extensions;
using Freem.Storage.Abstractions.Helpers;
using Freem.Storage.Abstractions.Helpers.Extensions;

namespace Freem.Entities.UseCases.Records;

internal sealed class UpdateRecordUseCase 
    : IEntitiesUseCase<UpdateRecordRequest, UpdateRecordResponse, UpdateRecordErrorCode>
{
    private readonly IDistributedLocker _locker;
    private readonly IRecordsRepository _repository;
    private readonly EventProducer _eventProducer;
    private readonly StorageTransactionRunner _transactionRunner;

    public UpdateRecordUseCase(
        IDistributedLocker locker,
        IRecordsRepository repository, 
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

    public async Task<UpdateRecordResponse> ExecuteAsync(
        UseCaseExecutionContext context, UpdateRecordRequest request,
        CancellationToken cancellationToken = default)
    {
        context.ThrowsIfUnauthorized();

        if (!request.HasChanges())
            return UpdateRecordResponse.CreateFailure(UpdateRecordErrorCode.NothingToUpdate);
        
        await using var @lock = await _locker.LockAsync(Lock.Prefix + request.Id, cancellationToken);
        
        var ids = new RecordAndUserIdentifiers(request.Id, context.UserId);
        var result = await _repository.FindByMultipleIdAsync(ids, cancellationToken);
        if (!result.Founded)
            return UpdateRecordResponse.CreateFailure(UpdateRecordErrorCode.RecordNotFound);
        
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
        
        return UpdateRecordResponse.CreateSuccess();
    }

    private async Task RunTransactionAsync(Record record, CancellationToken cancellationToken = default)
    {
        await _transactionRunner.RunAsync(async () =>
        {
            await _repository.UpdateAsync(record, cancellationToken);
            await _eventProducer.PublishAsync(record.BuildUpdatedEvent, cancellationToken);
        }, cancellationToken);
    }

    private static UpdateRecordResponse ProcessNotFoundRelatedException(NotFoundRelatedException ex)
    {
        if (ex.RelatedIds.HasActivitiesIdentifiers())
            return UpdateRecordResponse.CreateFailure(UpdateRecordErrorCode.RelatedActivitiesNotFound);
        if (ex.RelatedIds.HasTagsIdentifiers())
            return UpdateRecordResponse.CreateFailure(UpdateRecordErrorCode.RelatedTagsNotFound);

        return UpdateRecordResponse.CreateFailure(UpdateRecordErrorCode.RelatedUnknownNotFound);
    }
}