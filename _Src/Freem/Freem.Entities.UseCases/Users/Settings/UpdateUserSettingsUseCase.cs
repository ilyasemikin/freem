using Freem.Domain;
using Freem.Domain.Entities.Users;
using Freem.Domain.Storage.Repositories.Users;
using Freem.Entities.Events.Producer.Implementations;
using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts.Users.Settings.Update;
using Freem.Storage.Abstractions.Helpers;
using Freem.Storage.Abstractions.Helpers.Extensions;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Entities.UseCases.Users.Settings;

internal sealed class UpdateUserSettingsUseCase :
    IRequestHandler<HandlerContext, UpdateUserSettingsRequest, Result<Error>>
{
    private readonly IUserRepository _repository;
    private readonly EventProducer _eventProducer;
    private readonly StorageTransactionRunner _transactionRunner;

    public UpdateUserSettingsUseCase(
        IUserRepository repository, 
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

    public async Task<Result<Error>> HandleAsync(
        HandlerContext context, UpdateUserSettingsRequest request,
        CancellationToken cancellationToken = default)
    {
        if (!request.HasChanges())
            return DomainErrorCatalog.Factories.CreateNothingToDo();
        
        var result = await _repository.FindByIdAsync(context.UserId, cancellationToken);
        if (!result.Success)
        {
            return result.Error.Type == DomainErrorCatalog.Types.EntityNotFound
                ? DomainErrorCatalog.Factories.CreateUserNotRegistered(context.UserId)
                : result.Error;
        }

        var user = result.Value;
        var settings = new UserSettings()
        {
            DayUtcOffset = request.UtcOffset.Value
        };
        
        user.Settings = settings;

        return await _transactionRunner.RunAsync(async () =>
        {
            var repositoryResult = await _repository.UpdateAsync(user, cancellationToken);
            if (!repositoryResult.Success)
                return repositoryResult;

            return await _eventProducer.PublishAsync(user.BuildSettingsChangedEvent, cancellationToken);
        }, cancellationToken);
    }
}