using Freem.Entities.Storage.Abstractions.Base.Write;
using Freem.Entities.Storage.Abstractions.Exceptions;
using Freem.Entities.Tags;
using Freem.Entities.Tags.Identifiers;
using Freem.Entities.UseCases.Events.Abstractions;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.Tags.Create.Models;
using Freem.Identifiers.Abstractions.Generators;
using Freem.Storage.Abstractions.Helpers;
using Freem.Storage.Abstractions.Helpers.Extensions;

namespace Freem.Entities.UseCases.Tags.Create;

internal sealed class CreateTagUseCase : IUseCase<CreateTagRequest, CreateTagResponse>
{
    private readonly IIdentifierGenerator<TagIdentifier> _identifierGenerator;
    private readonly ICreateRepository<Tag, TagIdentifier> _repository;
    private readonly IEventProducer _eventProducer;
    private readonly StorageTransactionRunner _transactionRunner;

    public CreateTagUseCase(
        IIdentifierGenerator<TagIdentifier> identifierGenerator, 
        ICreateRepository<Tag, TagIdentifier> repository, 
        IEventProducer eventProducer, 
        StorageTransactionRunner transactionRunner)
    {
        ArgumentNullException.ThrowIfNull(identifierGenerator);
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(eventProducer);
        ArgumentNullException.ThrowIfNull(transactionRunner);
        
        _identifierGenerator = identifierGenerator;
        _repository = repository;
        _eventProducer = eventProducer;
        _transactionRunner = transactionRunner;
    }

    public async Task<CreateTagResponse> ExecuteAsync(
        UseCaseExecutionContext context, CreateTagRequest request,
        CancellationToken cancellationToken = default)
    {
        context.ThrowsIfUnauthorized();
        
        var id = _identifierGenerator.Generate();
        var tag = new Tag(id, context.UserId, request.Name);

        try
        {
            await RunTransactionAsync(tag, cancellationToken);
        }
        catch (DuplicateKeyStorageException ex)
        {
            return ProcessDuplicateKeyStorageException(ex);
        }

        return CreateTagResponse.CreateSuccess(tag);
    }

    private async Task RunTransactionAsync(Tag tag, CancellationToken cancellationToken = default)
    {
        await _transactionRunner.RunAsync(async () =>
        {
            await _repository.CreateAsync(tag, cancellationToken);
            await _eventProducer.PublishAsync(tag.BuildCreatedEvent, cancellationToken);
        }, cancellationToken);
    }

    private static CreateTagResponse ProcessDuplicateKeyStorageException(DuplicateKeyStorageException ex)
    {
        return ex.Code switch
        {
            DuplicateKeyStorageException.ErrorCode.DuplicateTagName => CreateTagResponse.CreateFailure(CreateTagErrorCode.TagNameAlreadyExists),
            _ => CreateTagResponse.CreateFailure(CreateTagErrorCode.UnknownError, ex.Message)
        };
    }
}