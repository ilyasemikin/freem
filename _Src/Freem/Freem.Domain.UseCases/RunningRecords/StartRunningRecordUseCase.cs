using Freem.Domain.Entities.RunningRecords;
using Freem.Domain.RunningRecords;
using Freem.Domain.Storage.Repositories.RunningRecords;
using Freem.Domain.Events.Producer.Implementations;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.RunningRecords.Start;
using Freem.Domain.UseCases.Contracts.RunningRecords.Stop;
using Freem.Locking;
using Freem.Locking.Extensions;
using Freem.Storage.Transactions.Helpers;
using Freem.Storage.Transactions.Helpers.Extensions;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Domain.UseCases.RunningRecords;

internal sealed class StartRunningRecordUseCase 
    : IRequestHandler<HandlerContext, StartRunningRecordRequest, Result<StartRunningRecordResponse, Error>>
{
    private readonly IDistributedLocker _locker;
    private readonly IRunningRecordRepository _repository;
    private readonly IRequestMediator<HandlerContext> _mediator;
    private readonly EventProducer _eventProducer;
    private readonly StorageTransactionRunner _transactionRunner;

    public StartRunningRecordUseCase(
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

    public async Task<Result<StartRunningRecordResponse, Error>> HandleAsync(
        HandlerContext context, StartRunningRecordRequest request,
        CancellationToken cancellationToken = default)
    {
        var record = new RunningRecord(context.UserId, request.Activities, request.Tags, request.StartAt)
        {
            Name = request.Name,
            Description = request.Description
        };
        
        var id = RunningRecordId.FromUserId(context.UserId);
        var currentResult = await _repository.FindByIdAsync(id, cancellationToken);
        if (currentResult.Success)
        {
            var stopRunningRecordRequest = new StopRunningRecordRequest(record.StartAt);
            var stopResult = await _mediator.ExecuteAsync<StopRunningRecordRequest, Result<Error>>(context, stopRunningRecordRequest, cancellationToken);
            if (!stopResult.Success)
                return stopResult.Error;
        }

        await using var @lock = await _locker.LockAsync(Lock.Prefix + context.UserId, cancellationToken);

        var writeResult = await RunTransactionAsync(context, record, cancellationToken);
        if (!writeResult.Success)
            return writeResult.Error;
        
        return new StartRunningRecordResponse(record);
    }

    private async Task<Result<Error>> RunTransactionAsync(
        HandlerContext context, RunningRecord record, 
        CancellationToken cancellationToken = default)
    {
        return await _transactionRunner.RunAsync(async () =>
        {
            var repositoryResult = await _repository.CreateAsync(record, cancellationToken);
            if (!repositoryResult.Success)
                return repositoryResult;

            return await _eventProducer.PublishAsync(record.BuildStartedEvent, cancellationToken);
        }, cancellationToken);
    }
}