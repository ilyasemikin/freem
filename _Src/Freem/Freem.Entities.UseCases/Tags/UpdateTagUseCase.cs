using Freem.Domain;
using Freem.Domain.Entities.Tags;
using Freem.Domain.Storage.Models.Ids;
using Freem.Domain.Storage.Repositories.Tags;
using Freem.Entities.Events.Producer.Implementations;
using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts.Tags.Update;
using Freem.Locking.Abstractions;
using Freem.Locking.Abstractions.Extensions;
using Freem.Storage.Abstractions.Helpers;
using Freem.Storage.Abstractions.Helpers.Extensions;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Entities.UseCases.Tags;

internal sealed class UpdateTagUseCase 
    : IRequestHandler<HandlerContext, UpdateTagRequest, Result<Error>>
{
    private readonly IDistributedLocker _locker;
    private readonly ITagRepository _repository;
    private readonly EventProducer _eventProducer;
    private readonly StorageTransactionRunner _transactionRunner;

    public UpdateTagUseCase(
        IDistributedLocker locker,
        ITagRepository repository, 
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
        HandlerContext context, UpdateTagRequest request,
        CancellationToken cancellationToken = default)
    {
        if (!request.HasChanges())
            return DomainErrorCatalog.Factories.CreateNothingToDo();
        
        await using var @lock = await _locker.LockAsync(Lock.Prefix + request.Id, cancellationToken);
        
        var ids = new TagAndUserIds(request.Id, context.UserId);
        var result = await _repository.FindByMultipleIdAsync(ids, cancellationToken);
        if (!result.Success)
            return result.Error;

        var tag = result.Value;
        if (request.Name is not null)
            tag.Name = request.Name;

        return await RunTransactionAsync(tag, cancellationToken);
    }

    private async Task<Result<Error>> RunTransactionAsync(Tag tag, CancellationToken cancellationToken = default)
    {
        return await _transactionRunner.RunAsync(async () =>
        {
            var repositoryResult = await _repository.UpdateAsync(tag, cancellationToken);
            if (!repositoryResult.Success)
                return repositoryResult;

            return await _eventProducer.PublishAsync(tag.BuildUpdatedEvent, cancellationToken);
        }, cancellationToken);
    }
}