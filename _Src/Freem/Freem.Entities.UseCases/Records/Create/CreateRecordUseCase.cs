using Freem.Entities.Records;
using Freem.Entities.Records.Identifiers;
using Freem.Entities.Storage.Abstractions.Base.Write;
using Freem.Entities.UseCases.Events.Abstractions;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.Records.Create.Models;
using Freem.Identifiers.Abstractions.Generators;
using Freem.Storage.Abstractions.Helpers;
using Freem.Storage.Abstractions.Helpers.Extensions;

namespace Freem.Entities.UseCases.Records.Create;

internal sealed class CreateRecordUseCase : IUseCase<CreateRecordRequest, CreateRecordResponse>
{
    private readonly IIdentifierGenerator<RecordIdentifier> _identifierGenerator;
    private readonly ICreateRepository<Record, RecordIdentifier> _repository;
    private readonly IEventProducer _eventProducer;
    private readonly StorageTransactionRunner _transactionRunner;

    public CreateRecordUseCase(
        IIdentifierGenerator<RecordIdentifier> identifierGenerator, 
        ICreateRepository<Record, RecordIdentifier> repository, 
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

        await _transactionRunner.RunAsync(async () =>
        {
            await _repository.CreateAsync(record, cancellationToken);
            await _eventProducer.PublishAsync(eventId => record.BuildCreatedEvent(eventId), cancellationToken);
        }, cancellationToken);

        return new CreateRecordResponse(record);
    }
}