using Freem.Entities.Bus.Events.Abstractions;
using Freem.Entities.Storage.Abstractions.Models.Identifiers;
using Freem.Entities.Storage.Abstractions.Repositories;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Contracts.Tags.Remove;
using Freem.Storage.Abstractions.Helpers;
using Freem.Storage.Abstractions.Helpers.Extensions;

namespace Freem.Entities.UseCases.Tags;

internal sealed class RemoveTagUseCase 
    : IEntitiesUseCase<RemoveTagRequest, RemoveTagResponse, RemoveTagErrorCode>
{
    private readonly ITagsRepository _repository;
    private readonly IEventProducer _eventProducer;
    private readonly StorageTransactionRunner _transactionRunner;

    public RemoveTagUseCase(
        ITagsRepository repository, 
        IEventProducer eventProducer, 
        StorageTransactionRunner transactionRunner)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(eventProducer);
        ArgumentNullException.ThrowIfNull(transactionRunner);
        
        _repository = repository;
        _eventProducer = eventProducer;
        _transactionRunner = transactionRunner;
    }

    public async Task<RemoveTagResponse> ExecuteAsync(
        UseCaseExecutionContext context, RemoveTagRequest request,
        CancellationToken cancellationToken = default)
    {
        context.ThrowsIfUnauthorized();
        
        var ids = new TagAndUserIdentifiers(request.Id, context.UserId);
        var result = await _repository.FindByMultipleIdAsync(ids, cancellationToken);
        if (!result.Founded)
            return RemoveTagResponse.CreateFailure(RemoveTagErrorCode.TagNotFound);

        var tag = result.Entity;

        await _transactionRunner.RunAsync(async () =>
        {
            await _repository.DeleteAsync(tag.Id, cancellationToken);
            await _eventProducer.PublishAsync(eventId => tag.BuildRemovedEvent(eventId), cancellationToken);
        }, cancellationToken);

        return RemoveTagResponse.CreateSuccess();
    }
}