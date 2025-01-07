using Freem.Entities.Storage.Abstractions.Models.Identifiers;
using Freem.Entities.Storage.Abstractions.Repositories;
using Freem.Entities.UseCases.Events.Abstractions;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.Tags.Remove.Models;
using Freem.Storage.Abstractions.Helpers;
using Freem.Storage.Abstractions.Helpers.Extensions;

namespace Freem.Entities.UseCases.Tags.Remove;

internal sealed class RemoveTagUseCase : IUseCase<RemoveTagRequest>
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

    public async Task ExecuteAsync(
        UseCaseExecutionContext context, RemoveTagRequest request,
        CancellationToken cancellationToken = default)
    {
        context.ThrowsIfUnauthorized();
        
        var ids = new TagAndUserIdentifiers(request.Id, context.UserId);
        var result = await _repository.FindByMultipleIdAsync(ids, cancellationToken);
        if (!result.Founded)
            throw new Exception();

        var tag = result.Entity;

        await _transactionRunner.RunAsync(async () =>
        {
            await _repository.DeleteAsync(tag.Id, cancellationToken);
            await _eventProducer.PublishAsync(eventId => tag.BuildRemovedEvent(eventId), cancellationToken);
        }, cancellationToken);
    }
}