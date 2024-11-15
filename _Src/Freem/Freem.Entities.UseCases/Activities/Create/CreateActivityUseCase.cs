using Freem.Entities.Activities;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Activities.Models;
using Freem.Entities.Storage.Abstractions.Base.Write;
using Freem.Entities.UseCases.Events.Abstractions;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.Activities.Create.Models;
using Freem.Identifiers.Abstractions.Generators;
using Freem.Storage.Abstractions.Helpers;
using Freem.Storage.Abstractions.Helpers.Extensions;

namespace Freem.Entities.UseCases.Activities.Create;

internal sealed class CreateActivityUseCase : IUseCase<CreateActivityRequest, CreateActivityResponse>
{
    private const ActivityStatus.Value DefaultActivityStatus = ActivityStatus.Value.Active;

    private readonly IIdentifierGenerator<ActivityIdentifier> _identifierGenerator;
    private readonly ICreateRepository<Activity, ActivityIdentifier> _repository;
    private readonly IEventProducer _eventProducer;
    private readonly StorageTransactionRunner _transactionRunner;

    public CreateActivityUseCase(
        IIdentifierGenerator<ActivityIdentifier> identifierGenerator, 
        ICreateRepository<Activity, ActivityIdentifier> repository, 
        IEventProducer eventProducer, 
        StorageTransactionRunner transactionRunner)
    {
        ArgumentNullException.ThrowIfNull(identifierGenerator);
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(eventProducer);
        ArgumentNullException.ThrowIfNull(transactionRunner);

        _identifierGenerator = identifierGenerator;
        _repository = repository;
        _eventProducer = eventProducer;
        _transactionRunner = transactionRunner;
    }

    public async Task<CreateActivityResponse> ExecuteAsync(
        UseCaseExecutionContext context, CreateActivityRequest request, 
        CancellationToken cancellationToken = default)
    {
        context.ThrowsIfUnauthorized();
        
        var id = _identifierGenerator.Generate();
        var activity = new Activity(id, context.UserId, request.Name, request.Tags, DefaultActivityStatus);

        await _transactionRunner.RunAsync(async () =>
        {
            await _repository.CreateAsync(activity, cancellationToken);
            await _eventProducer.PublishAsync(eventId => activity.BuildCreatedEvent(eventId), cancellationToken);
        }, cancellationToken);

        return new CreateActivityResponse(activity);
    }
}