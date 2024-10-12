using Freem.Entities.Storage.Abstractions.Base.Write;
using Freem.Entities.Tags;
using Freem.Entities.Tags.Identifiers;
using Freem.Entities.UseCases.Events.Abstractions;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Tags.Create.Models;
using Freem.Identifiers.Abstractions.Generators;
using Freem.Storage.Abstractions.Helpers;
using Freem.Storage.Abstractions.Helpers.Extensions;

namespace Freem.Entities.UseCases.Tags.Create;

internal sealed class CreateTagUseCase : IUseCase<CreateTagRequest, CreateTagResponse>
{
    private readonly IIdentifierGenerator<TagIdentifier> _identifierGenerator;
    private readonly ICreateRepository<Tag, TagIdentifier> _repository;
    private readonly IEventPublisher _eventPublisher;
    private readonly StorageTransactionRunner _transactionRunner;

    public CreateTagUseCase(
        IIdentifierGenerator<TagIdentifier> identifierGenerator, 
        ICreateRepository<Tag, TagIdentifier> repository, 
        IEventPublisher eventPublisher, 
        StorageTransactionRunner transactionRunner)
    {
        ArgumentNullException.ThrowIfNull(identifierGenerator);
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(eventPublisher);
        ArgumentNullException.ThrowIfNull(transactionRunner);
        
        _identifierGenerator = identifierGenerator;
        _repository = repository;
        _eventPublisher = eventPublisher;
        _transactionRunner = transactionRunner;
    }

    public async Task<CreateTagResponse> ExecuteAsync(
        UseCaseExecutionContext context, CreateTagRequest request,
        CancellationToken cancellationToken = default)
    {
        var id = _identifierGenerator.Generate();
        var tag = new Tag(id, context.UserId, request.Name);

        await _transactionRunner.RunAsync(async () =>
        {
            await _repository.CreateAsync(tag, cancellationToken);
            await _eventPublisher.PublishAsync(eventId => tag.BuildCreatedEvent(eventId), cancellationToken);
        }, cancellationToken);

        return new CreateTagResponse(tag);
    }
}