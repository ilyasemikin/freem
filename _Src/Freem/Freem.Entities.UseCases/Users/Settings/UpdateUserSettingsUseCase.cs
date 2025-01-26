using Freem.Entities.Bus.Events.Abstractions;
using Freem.Entities.Storage.Abstractions.Repositories;
using Freem.Entities.UseCases.Contracts.Users.Settings.Update;
using Freem.Entities.Users;
using Freem.Storage.Abstractions.Helpers;
using Freem.Storage.Abstractions.Helpers.Extensions;
using Freem.UseCases.Abstractions;

namespace Freem.Entities.UseCases.Users.Settings;

internal sealed class UpdateUserSettingsUseCase :
    IUseCase<UseCaseExecutionContext, UpdateUserSettingsRequest, UpdateUserSettingsResponse, UpdateUserSettingsErrorCode>
{
    private readonly IUsersRepository _repository;
    private readonly IEventProducer _eventProducer;
    private readonly StorageTransactionRunner _transactionRunner;

    public UpdateUserSettingsUseCase(
        IUsersRepository repository, 
        IEventProducer eventProducer,
        StorageTransactionRunner transactionRunner)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(eventProducer);
        ArgumentNullException.ThrowIfNull(transactionRunner);
        
        _repository = repository;
        _eventProducer = eventProducer;
        _transactionRunner = transactionRunner;
    }

    public async Task<UpdateUserSettingsResponse> ExecuteAsync(
        UseCaseExecutionContext context, UpdateUserSettingsRequest request,
        CancellationToken cancellationToken = default)
    {
        context.ThrowsIfUnauthorized();

        if (!request.HasChanges())
            return UpdateUserSettingsResponse.CreateFailure(UpdateUserSettingsErrorCode.NothingToDo);
        
        var result = await _repository.FindByIdAsync(context.UserId, cancellationToken);
        if (!result.Founded)
            return UpdateUserSettingsResponse.CreateFailure(UpdateUserSettingsErrorCode.UserNotFound);

        var user = result.Entity;
        var settings = new UserSettings()
        {
            UtcOffset = request.UtcOffset.Value
        };
        
        user.Settings = settings;

        await _transactionRunner.RunAsync(async () =>
        {
            await _repository.UpdateAsync(user, cancellationToken);
            await _eventProducer.PublishAsync(user.BuildSettingsChangedEvent, cancellationToken);
        }, cancellationToken);
        
        return UpdateUserSettingsResponse.CreateSuccess();
    }
}