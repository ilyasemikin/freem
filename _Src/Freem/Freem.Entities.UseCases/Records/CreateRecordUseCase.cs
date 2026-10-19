using Freem.Domain.Activities.Extensions;
using Freem.Domain.Entities.Records;
using Freem.Domain.Records;
using Freem.Domain.Storage.Exceptions;
using Freem.Domain.Storage.Repositories.Records;
using Freem.Domain.Tags.Extensions;
using Freem.Entities.Events.Producer.Implementations;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Contracts.Records.Create;
using Freem.Storage.Abstractions.Helpers;
using Freem.Storage.Abstractions.Helpers.Extensions;

namespace Freem.Entities.UseCases.Records;

internal sealed class CreateRecordUseCase 
    : IEntitiesUseCase<CreateRecordRequest, CreateRecordResponse, CreateRecordErrorCode>
{
    private readonly IRecordRepository _repository;
    private readonly EventProducer _eventProducer;
    private readonly StorageTransactionRunner _transactionRunner;

    public CreateRecordUseCase(
        IRecordRepository repository, 
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

    public async Task<CreateRecordResponse> ExecuteAsync(
        UseCaseExecutionContext context, CreateRecordRequest request,
        CancellationToken cancellationToken = default)
    {
        context.ThrowsIfUnauthorized();
        
        var id = RecordId.Generate();
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
        if (ex.RelatedIds.HasActivityIds())
            return CreateRecordResponse.CreateFailure(CreateRecordErrorCode.RelatedActivitiesNotFound);
        if (ex.RelatedIds.HasTagIds())
            return CreateRecordResponse.CreateFailure(CreateRecordErrorCode.RelatedTagsNotFound);
        
        return CreateRecordResponse.CreateFailure(CreateRecordErrorCode.RelatedUnknownNotFound);
    }
}