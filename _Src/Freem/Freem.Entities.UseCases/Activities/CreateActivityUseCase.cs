using Freem.Entities.Activities;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Activities.Models;
using Freem.Entities.Bus.Events.Abstractions;
using Freem.Entities.Storage.Abstractions.Base.Write;
using Freem.Entities.Storage.Abstractions.Exceptions;
using Freem.Entities.Tags.Identifiers.Extensions;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Contracts.Activities.Create;
using Freem.Identifiers.Abstractions.Generators;
using Freem.Storage.Abstractions.Helpers;
using Freem.Storage.Abstractions.Helpers.Extensions;
using Freem.UseCases.Abstractions;

namespace Freem.Entities.UseCases.Activities;

internal sealed class CreateActivityUseCase : 
    IEntitiesUseCase<CreateActivityRequest, CreateActivityResponse, CreateActivityErrorCode>
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

        try
        {
            await RunTransactionAsync(activity, cancellationToken);
        }
        catch (NotFoundRelatedException ex)
        {
            return ProcessNotFoundRelatedException(ex);
        }

        return CreateActivityResponse.CreateSuccess(activity);
    }

    private async Task RunTransactionAsync(Activity activity, CancellationToken cancellationToken = default)
    {
        await _transactionRunner.RunAsync(async () =>
        {
            await _repository.CreateAsync(activity, cancellationToken);
            await _eventProducer.PublishAsync(activity.BuildCreatedEvent, cancellationToken);
        }, cancellationToken);
    }

    private static CreateActivityResponse ProcessNotFoundRelatedException(NotFoundRelatedException ex)
    {
        if (ex.RelatedIds.HasTagsIdentifiers())
            return CreateActivityResponse.CreateFailure(CreateActivityErrorCode.RelatedTagsNotFound);

        return CreateActivityResponse.CreateFailure(CreateActivityErrorCode.RelatedUnknownNotFound);
    }
}