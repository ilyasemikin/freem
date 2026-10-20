using Freem.Domain;
using Freem.Domain.Entities.RunningRecords;
using Freem.Domain.RunningRecords;
using Freem.Domain.Storage.Repositories.RunningRecords;
using Freem.Domain.Events.Producer.Implementations;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Records.Create;
using Freem.Domain.UseCases.Contracts.RunningRecords.Stop;
using Freem.Locking;
using Freem.Locking.Extensions;
using Freem.Storage.Transactions.Helpers;
using Freem.Storage.Transactions.Helpers.Extensions;
using Freem.Time.Models;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Domain.UseCases.RunningRecords;

internal sealed class StopRunningRecordUseCase 
    : IRequestHandler<HandlerContext, StopRunningRecordRequest, Result<Error>>
{
    private readonly IDistributedLocker _locker;
    private readonly IRunningRecordRepository _repository;
    private readonly IRequestMediator<HandlerContext> _mediator;
    private readonly EventProducer _eventProducer;
    private readonly StorageTransactionRunner _transactionRunner;

    public StopRunningRecordUseCase(
        IDistributedLocker locker,
        IRunningRecordRepository repository, 
        IRequestMediator<HandlerContext> mediator, 
        EventProducer eventProducer, 
        StorageTransactionRunner transactionRunner)
    {
        ArgumentNullException.ThrowIfNull(locker);
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(mediator);
        ArgumentNullException.ThrowIfNull(eventProducer);
        ArgumentNullException.ThrowIfNull(transactionRunner);

        _locker = locker;
        _repository = repository;
        _mediator = mediator;
        _eventProducer = eventProducer;
        _transactionRunner = transactionRunner;
    }

    public async Task<Result<Error>> HandleAsync(
        HandlerContext context, StopRunningRecordRequest request,
        CancellationToken cancellationToken = default)
    {
        await using var @lock = await _locker.LockAsync(Lock.Prefix + context.UserId, cancellationToken);
        
        var id = RunningRecordId.FromUserId(context.UserId);
        var result = await _repository.FindByIdAsync(id, cancellationToken);
        if (!result.Success)
            return result.Error;
        
        var record = result.Value;
        if (record.StartAt > request.EndAt)
            return DomainErrorCatalog.Factories.CreatePeriodIsInvalid($"EndAt must be greater than \"{record.StartAt:O}\".");
        
        var createRecordRequest = BuildCreateRecordRequest(record, request.EndAt);

        return await _transactionRunner.RunAsync(async () =>
        {
            var createResult = await _mediator.ExecuteAsync<CreateRecordRequest, Result<CreateRecordResponse, Error>>(context, createRecordRequest, cancellationToken);
            if (!createResult.Success)
                return createResult.Error;

            var deleteResult = await _repository.DeleteAsync(record.Id, cancellationToken);
            if (!deleteResult.Success)
                return deleteResult;

            return await _eventProducer.PublishAsync(eventId => record.BuildStoppedEvent(eventId), cancellationToken);
        }, cancellationToken);
    }
    
    private static CreateRecordRequest BuildCreateRecordRequest(RunningRecord record, DateTimeOffset endAt)
    {
        var period = new DateTimePeriod(record.StartAt, endAt);

        return new CreateRecordRequest(period, record.Activities)
        {
            Name = record.Name,
            Description = record.Description,
            
            Tags = record.Tags,
        };
    }
}