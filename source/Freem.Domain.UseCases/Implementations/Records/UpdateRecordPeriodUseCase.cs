using Freem.Domain.Entities.Records;
using Freem.Domain.Storage.Models.Ids;
using Freem.Domain.Storage.Repositories.Records;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Records.UpdatePeriod;
using Freem.Domain.UseCases.Implementations.Events;
using Freem.Domain.UseCases.Implementations.Users.Settings;
using Freem.Domain.Users.Models;
using Freem.Storage.Transactions.Implementations;
using Freem.Storage.Transactions.Implementations.Extensions;
using Utilities.Errors;
using Utilities.Locking;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Domain.UseCases.Implementations.Records;

internal sealed class UpdateRecordPeriodUseCase
    : IRequestHandler<HandlerContext, UpdateRecordPeriodRequest, Result<Error>>
{
    private readonly ILocker _locker;
    private readonly IRecordRepository _repository;
    private readonly EventProducer _eventProducer;
    private readonly StorageTransactionRunner _transactionRunner;
    private readonly IRequestMediator<HandlerContext> _mediator;

    public UpdateRecordPeriodUseCase(
        ILocker locker,
        IRecordRepository repository,
        EventProducer eventProducer,
        StorageTransactionRunner transactionRunner,
        IRequestMediator<HandlerContext> mediator)
    {
        ArgumentNullException.ThrowIfNull(locker);
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(eventProducer);
        ArgumentNullException.ThrowIfNull(transactionRunner);
        ArgumentNullException.ThrowIfNull(mediator);

        _locker = locker;
        _repository = repository;
        _eventProducer = eventProducer;
        _transactionRunner = transactionRunner;
        _mediator = mediator;
    }

    public async Task<Result<Error>> HandleAsync(
        HandlerContext context, UpdateRecordPeriodRequest request,
        CancellationToken cancellationToken = default)
    {
        if (!request.HasChanges())
            return DomainErrorCatalog.Factories.CreateNothingToDo();

        var newTimeZoneOffset = (TimeZoneOffset?)null;
        if (request.Period is not null)
        {
            var durationError = RecordPeriodsFactory.ValidateDuration(request.Period);
            if (durationError is not null)
                return durationError;

            // Any change of the period time takes the current offset of the user
            var settingsResult = await _mediator.GetUserSettingsAsync(context, cancellationToken);
            if (!settingsResult.Success)
                return settingsResult.Error;

            newTimeZoneOffset = settingsResult.Value.TimeZoneOffset;
        }

        var lockResult = await _locker.LockAsync(Lock.Prefix + request.RecordId, cancellationToken);
        if (!lockResult.Success)
            return lockResult.Error;

        await using var @lock = lockResult.Value;

        var ids = new RecordAndUserIds(request.RecordId, context.UserId);
        var result = await _repository.FindByMultipleIdAsync(ids, cancellationToken);
        if (!result.Success)
            return result.Error;

        var record = result.Value;

        var current = record.Periods.FirstOrDefault(p => p.Id.Equals(request.PeriodId));
        if (current is null)
            return DomainErrorCatalog.Factories.CreateRecordPeriodNotFound(request.PeriodId);

        var period = request.Period is not null ? request.Period.Value : current.Period;
        var description = request.Description is not null ? request.Description.Value : current.Description;
        var timeZoneOffset = newTimeZoneOffset ?? current.TimeZoneOffset;
        var updated = new RecordPeriod(current.Id, period, timeZoneOffset, description);
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
