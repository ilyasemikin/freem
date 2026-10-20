using Freem.Domain.Entities.Records;
using Freem.Domain.Records;
using Freem.Domain.Storage.Repositories.Records;
using Freem.Entities.Events.Producer.Implementations;
using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts.Records.Create;
using Freem.Storage.Abstractions.Helpers;
using Freem.Storage.Abstractions.Helpers.Extensions;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Entities.UseCases.Records;

internal sealed class CreateRecordUseCase 
    : IRequestHandler<HandlerContext, CreateRecordRequest, Result<CreateRecordResponse, Error>>
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

    public async Task<Result<CreateRecordResponse, Error>> HandleAsync(
        HandlerContext context, CreateRecordRequest request,
        CancellationToken cancellationToken = default)
    {
        var id = RecordId.Generate();
        var record = new Record(id, context.UserId, request.Activities, request.Tags, request.Period)
        {
            Name = request.Name,
            Description = request.Description
        };

        var writeResult = await RunTransactionAsync(record, cancellationToken);
        if (!writeResult.Success)
            return writeResult.Error;

        return new CreateRecordResponse(record);
    }

    private async Task<Result<Error>> RunTransactionAsync(Record record, CancellationToken cancellationToken = default)
    {
        return await _transactionRunner.RunAsync(async () =>
        {
            var repositoryResult = await _repository.CreateAsync(record, cancellationToken);
            if (!repositoryResult.Success)
                return repositoryResult;

            return await _eventProducer.PublishAsync(record.BuildCreatedEvent, cancellationToken);
        }, cancellationToken);
    }
}