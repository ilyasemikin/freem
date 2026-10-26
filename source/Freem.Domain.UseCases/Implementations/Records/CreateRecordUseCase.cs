using Freem.Domain.Entities.Records;
using Freem.Domain.Records;
using Freem.Domain.Storage.Repositories.Records;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Records.Create;
using Freem.Domain.UseCases.Implementations.Events;
using Freem.Domain.UseCases.Implementations.Users.Settings;
using Freem.Storage.Transactions.Implementations;
using Freem.Storage.Transactions.Implementations.Extensions;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Domain.UseCases.Implementations.Records;

internal sealed class CreateRecordUseCase 
    : IRequestHandler<HandlerContext, CreateRecordRequest, Result<CreateRecordResponse, Error>>
{
    private readonly IRecordRepository _repository;
    private readonly EventProducer _eventProducer;
    private readonly StorageTransactionRunner _transactionRunner;
    private readonly IRequestMediator<HandlerContext> _mediator;

    public CreateRecordUseCase(
        IRecordRepository repository, 
        EventProducer eventProducer, 
        StorageTransactionRunner transactionRunner,
        IRequestMediator<HandlerContext> mediator)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(eventProducer);
        ArgumentNullException.ThrowIfNull(transactionRunner);
        ArgumentNullException.ThrowIfNull(mediator);
        
        _repository = repository;
        _eventProducer = eventProducer;
        _transactionRunner = transactionRunner;
        _mediator = mediator;
    }

    public async Task<Result<CreateRecordResponse, Error>> HandleAsync(
        HandlerContext context, CreateRecordRequest request,
        CancellationToken cancellationToken = default)
    {
        var settingsResult = await _mediator.GetUserSettingsAsync(context, cancellationToken);
        if (!settingsResult.Success)
            return settingsResult.Error;

        var periodsResult = RecordPeriodsFactory.Create(request.Periods, settingsResult.Value.TimeZoneOffset);
        if (!periodsResult.Success)
            return periodsResult.Error;

        var id = RecordId.Generate();
        var record = new Record(id, context.UserId, request.Activities, request.Tags, periodsResult.Value, request.Source)
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

            return await _eventProducer.PublishAsync(record.UserId, record.BuildCreatedEvent, cancellationToken);
        }, cancellationToken);
    }
}