using Freem.Domain.Storage.Models.Ids;
using Freem.Domain.Storage.Repositories.Tags;
using Freem.Entities.Events.Producer.Implementations;
using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts.Tags.Remove;
using Freem.Storage.Abstractions.Helpers;
using Freem.Storage.Abstractions.Helpers.Extensions;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Entities.UseCases.Tags;

internal sealed class RemoveTagUseCase 
    : IRequestHandler<HandlerContext, RemoveTagRequest, Result<Error>>
{
    private readonly ITagRepository _repository;
    private readonly EventProducer _eventProducer;
    private readonly StorageTransactionRunner _transactionRunner;

    public RemoveTagUseCase(
        ITagRepository repository, 
        EventProducer eventProducer, 
        StorageTransactionRunner transactionRunner)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(eventProducer);
        ArgumentNullException.ThrowIfNull(transactionRunner);
        
        _repository = repository;
        _eventProducer = eventProducer;
        _transactionRunner = transactionRunner;
    }

    public async Task<Result<Error>> HandleAsync(
        HandlerContext context, RemoveTagRequest request,
        CancellationToken cancellationToken = default)
    {
        var ids = new TagAndUserIds(request.Id, context.UserId);
        var result = await _repository.FindByMultipleIdAsync(ids, cancellationToken);
        if (!result.Success)
            return result.Error;

        var tag = result.Value;

        return await _transactionRunner.RunAsync(async () =>
        {
            var repositoryResult = await _repository.DeleteAsync(tag.Id, cancellationToken);
            if (!repositoryResult.Success)
                return repositoryResult;

            return await _eventProducer.PublishAsync(eventId => tag.BuildRemovedEvent(eventId), cancellationToken);
        }, cancellationToken);
    }
}