using Freem.Domain.Activities;
using Freem.Domain.Activities.Models;
using Freem.Domain.Entities.Activities;
using Freem.Domain.Storage.Repositories.Activities;
using Freem.Domain.Events.Producer.Implementations;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Activities.Create;
using Freem.Storage.Transactions.Helpers;
using Freem.Storage.Transactions.Helpers.Extensions;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Domain.UseCases.Activities;

internal sealed class CreateActivityUseCase : 
    IRequestHandler<HandlerContext, CreateActivityRequest, Result<CreateActivityResponse, Error>>
{
    private static readonly ActivityStatus DefaultActivityStatus = ActivityStatus.Active;

    private readonly IActivityRepository _repository;
    private readonly EventProducer _eventProducer;
    private readonly StorageTransactionRunner _transactionRunner;

    public CreateActivityUseCase(
        IActivityRepository repository, 
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

    public async Task<Result<CreateActivityResponse, Error>> HandleAsync(
        HandlerContext context, CreateActivityRequest request, 
        CancellationToken cancellationToken = default)
    {
        var id = ActivityId.Generate();
        var activity = new Activity(id, context.UserId, request.Name, request.Tags, DefaultActivityStatus);

        var result = await RunTransactionAsync(activity, cancellationToken);
        if (!result.Success)
            return result.Error;

        return new CreateActivityResponse(activity);
    }

    private async Task<Result<Error>> RunTransactionAsync(Activity activity, CancellationToken cancellationToken = default)
    {
        return await _transactionRunner.RunAsync(async () =>
        {
            var result = await _repository.CreateAsync(activity, cancellationToken);
            if (!result.Success)
                return result;

            return await _eventProducer.PublishAsync(activity.BuildCreatedEvent, cancellationToken);
        }, cancellationToken);
    }
}