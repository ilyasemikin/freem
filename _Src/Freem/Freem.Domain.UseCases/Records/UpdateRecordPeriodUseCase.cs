using Freem.Domain.Entities.Records;
using Freem.Domain.Records;
using Freem.Domain.Storage.Models.Ids;
using Freem.Domain.Storage.Repositories.Records;
using Freem.Domain.UseCases.Events;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Records.UpdatePeriod;
using Freem.Locking;
using Freem.Locking.Extensions;
using Freem.Storage.Transactions.Helpers;
using Freem.Storage.Transactions.Helpers.Extensions;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Domain.UseCases.Records;

internal sealed class UpdateRecordPeriodUseCase
    : IRequestHandler<HandlerContext, UpdateRecordPeriodRequest, Result<Error>>
{
    private readonly IDistributedLocker _locker;
    private readonly IRecordRepository _repository;
    private readonly EventProducer _eventProducer;
    private readonly StorageTransactionRunner _transactionRunner;

    public UpdateRecordPeriodUseCase(
        IDistributedLocker locker,
        IRecordRepository repository,
        EventProducer eventProducer,
        StorageTransactionRunner transactionRunner)
    {
        ArgumentNullException.ThrowIfNull(locker);
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(eventProducer);
        ArgumentNullException.ThrowIfNull(transactionRunner);

        _locker = locker;
        _repository = repository;
        _eventProducer = eventProducer;
        _transactionRunner = transactionRunner;
    }

    public async Task<Result<Error>> HandleAsync(
        HandlerContext context, UpdateRecordPeriodRequest request,
        CancellationToken cancellationToken = default)
    {
        if (!request.HasChanges())
            return DomainErrorCatalog.Factories.CreateNothingToDo();

        if (request.Period is not null)
        {
            var durationError = RecordPeriodsFactory.ValidateDuration(request.Period);
            if (durationError is not null)
                return durationError;
        }

        await using var @lock = await _locker.LockAsync(Lock.Prefix + request.RecordId, cancellationToken);

        var ids = new RecordAndUserIds(request.RecordId, context.UserId);
        var result = await _repository.FindByMultipleIdAsync(ids, cancellationToken);
        if (!result.Success)
            return result.Error;

        var record = result.Value;

        var current = record.Periods.FirstOrDefault(p => p.Id.Equals(request.PeriodId));
        if (current is null)
            return DomainErrorCatalog.Factories.CreateEntityNotFound(request.PeriodId);

        var period = request.Period is not null ? request.Period.Value : current.Period;
        var description = request.Description is not null ? request.Description.Value : current.Description;
        var updated = new RecordPeriod(current.Id, period, description);
        if (!record.Periods.TryReplace(updated))
            return DomainErrorCatalog.Factories.CreatePeriodIsInvalid("Period must not overlap other periods of the record.");

        return await RunTransactionAsync(record, cancellationToken);
    }

    private async Task<Result<Error>> RunTransactionAsync(Record record, CancellationToken cancellationToken)
    {
        return await _transactionRunner.RunAsync(async () =>
        {
            var repositoryResult = await _repository.UpdateAsync(record, cancellationToken);
            if (!repositoryResult.Success)
                return repositoryResult;

            return await _eventProducer.PublishAsync(record.UserId, record.BuildUpdatedEvent, cancellationToken);
        }, cancellationToken);
    }
}
