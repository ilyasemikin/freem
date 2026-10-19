using Freem.Domain.Entities.Users;
using Freem.Domain.Storage.Exceptions;
using Freem.Domain.Storage.Repositories.Users;
using Freem.Entities.Events.Producer.Implementations;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Contracts.Users.Register;
using Freem.Storage.Abstractions.Helpers;
using Freem.Storage.Abstractions.Helpers.Extensions;

namespace Freem.Entities.UseCases.Users;

internal sealed class RegisterUserUseCase 
    : IEntitiesUseCase<RegisterUserRequest, RegisterUserResponse, RegisterUserErrorCode>
{
    private readonly IUserRepository _repository;
    private readonly EventProducer _eventProducer;
    private readonly StorageTransactionRunner _transactionRunner;

    public RegisterUserUseCase(
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

    public async Task<RegisterUserResponse> ExecuteAsync(
        UseCaseExecutionContext context, RegisterUserRequest request,
        CancellationToken cancellationToken = default)
    {
        context.ThrowsIfUnauthorized();

        var id = context.UserId;
        var user = new User(id, request.Nickname);

        try
        {
            await RunTransactionAsync(user, cancellationToken);
        }
        catch (DuplicateKeyStorageException ex)
        {
            return RegisterUserResponse.CreateAlreadyRegisteredFailure(ex.Message);
        }

        return RegisterUserResponse.CreateSuccess(id);
    }

    private async Task RunTransactionAsync(User user, CancellationToken cancellationToken = default)
    {
        await _transactionRunner.RunAsync(async () =>
        {
            await _repository.CreateAsync(user, cancellationToken);
            await _eventProducer.PublishAsync(user.BuildRegisteredEvent, cancellationToken);
        }, cancellationToken);
    }
}
