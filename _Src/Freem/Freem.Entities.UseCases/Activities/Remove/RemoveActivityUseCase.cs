using Freem.Entities.Storage.Abstractions.Models.Identifiers;
using Freem.Entities.Storage.Abstractions.Repositories;
using Freem.Entities.UseCases.Events.Abstractions;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Activities.Remove.Models;
using Freem.Entities.UseCases.Context;
using Freem.Storage.Abstractions.Helpers;
using Freem.Storage.Abstractions.Helpers.Extensions;

namespace Freem.Entities.UseCases.Activities.Remove;

internal sealed class RemoveActivityUseCase : IUseCase<RemoveActivityRequest>
{
    private readonly IActivitiesRepository _repository;
    private readonly IEventPublisher _eventPublisher;
    private readonly StorageTransactionRunner _transactionRunner;

    public RemoveActivityUseCase(
        IActivitiesRepository repository, 
        IEventPublisher eventPublisher, 
        StorageTransactionRunner transactionRunner)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(eventPublisher);
        ArgumentNullException.ThrowIfNull(transactionRunner);
        
        _repository = repository;
        _eventPublisher = eventPublisher;
        _transactionRunner = transactionRunner;
    }

    public async Task ExecuteAsync(
        UseCaseExecutionContext context, RemoveActivityRequest request, 
        CancellationToken cancellationToken = default)
    {
        var ids = new ActivityAndUserIdentifiers(request.Id, context.UserId);
        var result = await _repository.FindByMultipleIdAsync(ids, cancellationToken);
        if (!result.Founded)
            throw new Exception();

        var activity = result.Entity;

        await _transactionRunner.RunAsync(async () =>
        {
            await _repository.RemoveAsync(activity.Id, cancellationToken);
            await _eventPublisher.PublishAsync(eventId => activity.BuildRemovedEvent(eventId), cancellationToken);
        }, cancellationToken);
    }
}