using Freem.Entities.RunningRecords;
using Freem.Entities.Storage.Abstractions.Repositories;
using Freem.Entities.UseCases.Events.Abstractions;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.RunningRecords.Start.Models;
using Freem.Entities.UseCases.RunningRecords.Stop.Models;
using Freem.Locking.Abstractions;
using Freem.Locking.Abstractions.Extensions;
using Freem.Storage.Abstractions.Helpers;
using Freem.Storage.Abstractions.Helpers.Extensions;

namespace Freem.Entities.UseCases.RunningRecords.Start;

internal sealed class StartRunningRecordUseCase : IUseCase<StartRunningRecordRequest>
{
    private readonly IDistributedLocker _locker;
    private readonly IRunningRecordRepository _repository;
    private readonly IUseCaseExecutor _executor;
    private readonly IEventPublisher _eventPublisher;
    private readonly StorageTransactionRunner _transactionRunner;

    public StartRunningRecordUseCase(
        IDistributedLocker locker,
        IRunningRecordRepository repository, 
        IUseCaseExecutor executor, 
        IEventPublisher eventPublisher, 
        StorageTransactionRunner transactionRunner)
    {
        ArgumentNullException.ThrowIfNull(locker);
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(executor);
        ArgumentNullException.ThrowIfNull(eventPublisher);
        ArgumentNullException.ThrowIfNull(_transactionRunner);

        _locker = locker;
        _repository = repository;
        _executor = executor;
        _eventPublisher = eventPublisher;
        _transactionRunner = transactionRunner;
    }

    public async Task ExecuteAsync(
        UseCaseExecutionContext context, StartRunningRecordRequest request,
        CancellationToken cancellationToken = default)
    {
        await using var @lock = await _locker.LockAsync(Lock.Prefix + context.UserId, cancellationToken);
        
        var result = await _repository.FindByIdAsync(context.UserId, cancellationToken);
        if (result.Founded)
        {
            var stopRunningRecordRequest = new StopRunningRecordRequest(request.StartAt);
            await _executor.ExecuteAsync(context, stopRunningRecordRequest, cancellationToken);
        }

        var record = new RunningRecord(context.UserId, request.Activities, request.Tags, request.StartAt)
        {
            Name = request.Name,
            Description = request.Description
        };

        await _transactionRunner.RunAsync(async () =>
        {
            await _repository.CreateAsync(record, cancellationToken);
            await _eventPublisher.PublishAsync(eventId => record.BuildStartedEvent(eventId), cancellationToken);
        }, cancellationToken);
    }
}