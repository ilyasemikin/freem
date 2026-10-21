using Freem.Domain.Entities.Records;
using Freem.Domain.Records;
using Freem.Domain.Storage.Models.Ids;
using Freem.Domain.Storage.Repositories.Records;
using Freem.Domain.UseCases.Events;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Records.AddPeriod;
using Freem.Domain.UseCases.Users.Settings;
using Freem.Locking;
using Freem.Locking.Extensions;
using Freem.Storage.Transactions.Helpers;
using Freem.Storage.Transactions.Helpers.Extensions;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Domain.UseCases.Records;

internal sealed class AddRecordPeriodUseCase
    : IRequestHandler<HandlerContext, AddRecordPeriodRequest, Result<AddRecordPeriodResponse, Error>>
{
    private readonly IDistributedLocker _locker;
    private readonly IRecordRepository _repository;
    private readonly EventProducer _eventProducer;
    private readonly StorageTransactionRunner _transactionRunner;
    private readonly IRequestMediator<HandlerContext> _mediator;

    public AddRecordPeriodUseCase(
        IDistributedLocker locker,
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

    public async Task<Result<AddRecordPeriodResponse, Error>> HandleAsync(
        HandlerContext context, AddRecordPeriodRequest request,
        CancellationToken cancellationToken = default)
    {
        var durationError = RecordPeriodsFactory.ValidateDuration(request.Period);
        if (durationError is not null)
            return durationError;

        var settingsResult = await _mediator.GetUserSettingsAsync(context, cancellationToken);
        if (!settingsResult.Success)
            return settingsResult.Error;

        await using var @lock = await _locker.LockAsync(Lock.Prefix + request.RecordId, cancellationToken);

        var ids = new RecordAndUserIds(request.RecordId, context.UserId);
        var result = await _repository.FindByMultipleIdAsync(ids, cancellationToken);
        if (!result.Success)
            return result.Error;

        var record = result.Value;

        var periodId = RecordPeriodId.Generate();
        var period = new RecordPeriod(periodId, request.Period, settingsResult.Value.TimeZoneOffset, request.Description);
        if (!record.Periods.TryAdd(period))
            return DomainErrorCatalog.Factories.CreatePeriodIsInvalid("Period must not overlap other periods of the record.");

        var writeResult = await RunTransactionAsync(record, cancellationToken);
        if (!writeResult.Success)
            return writeResult.Error;

        return new AddRecordPeriodResponse(periodId);
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
