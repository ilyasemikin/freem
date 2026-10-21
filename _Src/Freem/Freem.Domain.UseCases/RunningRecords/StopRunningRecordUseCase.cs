using Freem.Domain;
using Freem.Domain.Entities.RunningRecords;
using Freem.Domain.RunningRecords;
using Freem.Domain.Storage.Repositories.RunningRecords;
using Freem.Domain.UseCases.Events;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Records.Create;
using Freem.Domain.UseCases.Contracts.RunningRecords.Stop;
using Freem.Storage.Transactions.Helpers;
using Freem.Storage.Transactions.Helpers.Extensions;
using Freem.Time.Models;
using Utilities.Errors;
using Utilities.Locking;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Domain.UseCases.RunningRecords;

internal sealed class StopRunningRecordUseCase 
    : IRequestHandler<HandlerContext, StopRunningRecordRequest, Result<Error>>
{
    private readonly ILocker _locker;
    private readonly IRunningRecordRepository _repository;
    private readonly IRequestMediator<HandlerContext> _mediator;
    private readonly EventProducer _eventProducer;
    private readonly StorageTransactionRunner _transactionRunner;

    public StopRunningRecordUseCase(
        ILocker locker,
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
        var lockResult = await _locker.LockAsync(Lock.Prefix + context.UserId, cancellationToken);
        if (!lockResult.Success)
            return lockResult.Error;

        await using var @lock = lockResult.Value;
        
        var id = RunningRecordId.FromUserId(context.UserId);
        var result = await _repository.FindByIdAsync(id, cancellationToken);
        if (!result.Success)
            return result.Error;
        
        var record = result.Value;
        if (record.StartAt > request.EndAt)
            return DomainErrorCatalog.Factories.CreatePeriodIsInvalid($"EndAt must be greater than \"{record.StartAt:O}\".");
        
        var period = new DateTimePeriod(record.StartAt, request.EndAt);
        var createRecordRequest = BuildCreateRecordRequest(record, period);

        return await _transactionRunner.RunAsync(async () =>
        {
            // Период нулевой длины (начало и конец совпали с точностью до миллисекунды): записи не создаём, запущенная запись просто останавливается
            if (period.Duration > TimeSpan.Zero)
            {
                var createResult = await _mediator.ExecuteAsync<CreateRecordRequest, Result<CreateRecordResponse, Error>>(context, createRecordRequest, cancellationToken);
                if (!createResult.Success)
                    return createResult.Error;
            }

            var deleteResult = await _repository.DeleteAsync(record.Id, cancellationToken);
            if (!deleteResult.Success)
                return deleteResult;

            return await _eventProducer.PublishAsync(record.UserId, eventId => record.BuildStoppedEvent(eventId), cancellationToken);
        }, cancellationToken);
    }
    
    private static CreateRecordRequest BuildCreateRecordRequest(RunningRecord record, DateTimePeriod period)
    {
        var recordPeriod = new NewRecordPeriod(period);
        var periods = new[] { recordPeriod };

        return new CreateRecordRequest(periods, record.Activities)
        {
            Name = record.Name,
            Description = record.Description,
            
            Tags = record.Tags,
        };
    }
}