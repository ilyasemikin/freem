using Freem.Domain.Entities.Tags;
using Freem.Domain.Storage.Repositories.Tags;
using Freem.Domain.Tags;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Tags.Create;
using Freem.Domain.UseCases.Implementations.Events;
using Freem.Storage.Transactions.Implementations;
using Freem.Storage.Transactions.Implementations.Extensions;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Domain.UseCases.Implementations.Tags;

internal sealed class CreateTagUseCase 
    : IRequestHandler<HandlerContext, CreateTagRequest, Result<CreateTagResponse, Error>>
{
    private readonly ITagRepository _repository;
    private readonly EventProducer _eventProducer;
    private readonly StorageTransactionRunner _transactionRunner;

    public CreateTagUseCase(
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

    public async Task<Result<CreateTagResponse, Error>> HandleAsync(
        HandlerContext context, CreateTagRequest request,
        CancellationToken cancellationToken = default)
    {
        var id = TagId.Generate();
        var tag = new Tag(id, context.UserId, request.Name);

        var writeResult = await RunTransactionAsync(tag, cancellationToken);
        if (!writeResult.Success)
            return writeResult.Error;

        return new CreateTagResponse(tag);
    }

    private async Task<Result<Error>> RunTransactionAsync(Tag tag, CancellationToken cancellationToken = default)
    {
        return await _transactionRunner.RunAsync(async () =>
        {
            var repositoryResult = await _repository.CreateAsync(tag, cancellationToken);
            if (!repositoryResult.Success)
                return repositoryResult;

            return await _eventProducer.PublishAsync(tag.UserId, tag.BuildCreatedEvent, cancellationToken);
        }, cancellationToken);
    }
}