using Freem.Domain.Activities.Identifiers.Extensions;
using Freem.Domain.Entities.Records;
using Freem.Domain.Records.Identifiers;
using Freem.Domain.Storage.Exceptions;
using Freem.Domain.Storage.Repositories.Records;
using Freem.Domain.Tags.Identifiers.Extensions;
using Freem.Entities.Events.Producer.Implementations;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Contracts.Records.Create;
using Freem.Identifiers.Abstractions.Generators;
using Freem.Storage.Abstractions.Helpers;
using Freem.Storage.Abstractions.Helpers.Extensions;

namespace Freem.Entities.UseCases.Records;

internal sealed class CreateRecordUseCase 
    : IEntitiesUseCase<CreateRecordRequest, CreateRecordResponse, CreateRecordErrorCode>
{
    private readonly IIdentifierGenerator<RecordIdentifier> _identifierGenerator;
    private readonly IRecordRepository _repository;
    private readonly EventProducer _eventProducer;
    private readonly StorageTransactionRunner _transactionRunner;

    public CreateRecordUseCase(
        IIdentifierGenerator<RecordIdentifier> identifierGenerator, 
        IRecordRepository repository, 
        EventProducer eventProducer, 
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

    public async Task<CreateRecordResponse> ExecuteAsync(
        UseCaseExecutionContext context, CreateRecordRequest request,
        CancellationToken cancellationToken = default)
    {
        context.ThrowsIfUnauthorized();
        
        var id = _identifierGenerator.Generate();
        var record = new Record(id, context.UserId, request.Activities, request.Tags, request.Period)
        {
            Name = request.Name,
            Description = request.Description
        };

        try
        {
            await RunTransactionAsync(record, cancellationToken);
        }
        catch (NotFoundRelatedException ex)
        {
            return ProcessNotFoundRelatedException(ex);
        }

        return CreateRecordResponse.CreateSuccess(record);
    }

    private async Task RunTransactionAsync(Record record, CancellationToken cancellationToken = default)
    {
        await _transactionRunner.RunAsync(async () =>
        {
            await _repository.CreateAsync(record, cancellationToken);
            await _eventProducer.PublishAsync(record.BuildCreatedEvent, cancellationToken);
        }, cancellationToken);
    }

    private static CreateRecordResponse ProcessNotFoundRelatedException(NotFoundRelatedException ex)
    {
        if (ex.RelatedIds.HasActivitiesIdentifiers())
            return CreateRecordResponse.CreateFailure(CreateRecordErrorCode.RelatedActivitiesNotFound);
        if (ex.RelatedIds.HasTagsIdentifiers())
            return CreateRecordResponse.CreateFailure(CreateRecordErrorCode.RelatedTagsNotFound);
        
        return CreateRecordResponse.CreateFailure(CreateRecordErrorCode.RelatedUnknownNotFound);
    }
}