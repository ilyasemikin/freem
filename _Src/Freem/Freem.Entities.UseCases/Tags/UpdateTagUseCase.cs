using Freem.Entities.Events.Producer.Implementations;
using Freem.Entities.Storage.Abstractions.Exceptions;
using Freem.Entities.Storage.Abstractions.Models.Identifiers;
using Freem.Entities.Storage.Abstractions.Repositories;
using Freem.Entities.Tags;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Contracts.Tags.Update;
using Freem.Locking.Abstractions;
using Freem.Locking.Abstractions.Extensions;
using Freem.Storage.Abstractions.Helpers;
using Freem.Storage.Abstractions.Helpers.Extensions;

namespace Freem.Entities.UseCases.Tags;

internal sealed class UpdateTagUseCase 
    : IEntitiesUseCase<UpdateTagRequest, UpdateTagResponse, UpdateTagErrorCode>
{
    private readonly IDistributedLocker _locker;
    private readonly ITagsRepository _repository;
    private readonly EventProducer _eventProducer;
    private readonly StorageTransactionRunner _transactionRunner;

    public UpdateTagUseCase(
        IDistributedLocker locker,
        ITagsRepository repository, 
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

    public async Task<UpdateTagResponse> ExecuteAsync(
        UseCaseExecutionContext context, UpdateTagRequest request,
        CancellationToken cancellationToken = default)
    {
        context.ThrowsIfUnauthorized();

        if (!request.HasChanges())
            return UpdateTagResponse.CreateFailure(UpdateTagErrorCode.NothingToUpdate);
        
        await using var @lock = await _locker.LockAsync(Lock.Prefix + request.Id, cancellationToken);
        
        var ids = new TagAndUserIdentifiers(request.Id, context.UserId);
        var result = await _repository.FindByMultipleIdAsync(ids, cancellationToken);
        if (!result.Founded)
            return UpdateTagResponse.CreateFailure(UpdateTagErrorCode.TagNotFound);

        var tag = result.Entity;
        if (request.Name is not null)
            tag.Name = request.Name;

        try
        {
            await RunTransactionAsync(tag, cancellationToken);
        }
        catch (DuplicateKeyStorageException ex)
        {
            return ProcessDuplicateKeyStorageException(ex);
        }

        return UpdateTagResponse.CreateSuccess();
    }

    private async Task RunTransactionAsync(Tag tag, CancellationToken cancellationToken = default)
    {
        await _transactionRunner.RunAsync(async () =>
        {
            await _repository.UpdateAsync(tag, cancellationToken);
            await _eventProducer.PublishAsync(tag.BuildUpdatedEvent, cancellationToken);
        }, cancellationToken);
    }

    private static UpdateTagResponse ProcessDuplicateKeyStorageException(DuplicateKeyStorageException ex)
    {
        return ex.Code switch
        {
            DuplicateKeyStorageException.ErrorCode.DuplicateTagName => UpdateTagResponse.CreateFailure(UpdateTagErrorCode.TagNameAlreadyExists),
            _ => UpdateTagResponse.CreateFailure(UpdateTagErrorCode.UnknownError, ex.Message)
        };
    }
}