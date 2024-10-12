using Freem.Entities.Records;
using Freem.Entities.Records.Identifiers;
using Freem.Entities.Storage.Abstractions.Base.Write;
using Freem.Entities.UseCases.Events.Abstractions;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Records.Create.Models;
using Freem.Identifiers.Abstractions.Generators;
using Freem.Storage.Abstractions.Helpers;
using Freem.Storage.Abstractions.Helpers.Extensions;

namespace Freem.Entities.UseCases.Records.Create;

internal sealed class CreateRecordUseCase : IUseCase<CreateRecordRequest, CreateRecordResponse>
{
    private readonly IIdentifierGenerator<RecordIdentifier> _identifierGenerator;
    private readonly ICreateRepository<Record, RecordIdentifier> _repository;
    private readonly IEventPublisher _eventPublisher;
    private readonly StorageTransactionRunner _transactionRunner;

    public CreateRecordUseCase(
        IIdentifierGenerator<RecordIdentifier> identifierGenerator, 
        ICreateRepository<Record, RecordIdentifier> repository, 
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

    public async Task<CreateRecordResponse> ExecuteAsync(
        UseCaseExecutionContext context, CreateRecordRequest request,
        CancellationToken cancellationToken = default)
    {
        var id = _identifierGenerator.Generate();
        var record = new Record(id, context.UserId, request.Activities, request.Tags, request.Period)
        {
            Name = request.Name,
            Description = request.Description
        };

        await _transactionRunner.RunAsync(async () =>
        {
            await _repository.CreateAsync(record, cancellationToken);
            await _eventPublisher.PublishAsync(eventId => record.BuildCreatedEvent(eventId), cancellationToken);
        }, cancellationToken);

        return new CreateRecordResponse(record);
    }
}