using Freem.Entities.Events.Producer.Implementations;
using Freem.Entities.Storage.Abstractions.Base.Write;
using Freem.Entities.Storage.Abstractions.Exceptions;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Contracts.Users.Register;
using Freem.Entities.Users;
using Freem.Entities.Users.Identifiers;
using Freem.Identifiers.Abstractions.Generators;
using Freem.Storage.Abstractions.Helpers;
using Freem.Storage.Abstractions.Helpers.Extensions;

namespace Freem.Entities.UseCases.Users;

internal sealed class RegisterUserUseCase 
    : IEntitiesUseCase<RegisterUserRequest, RegisterUserResponse, RegisterUserErrorCode>
{
    private readonly IIdentifierGenerator<UserIdentifier> _identifierGenerator;
    private readonly ICreateRepository<User, UserIdentifier> _repository;
    private readonly EventProducer _eventProducer;
    private readonly StorageTransactionRunner _transactionRunner;

    public RegisterUserUseCase(
        IIdentifierGenerator<UserIdentifier> identifierGenerator, 
        ICreateRepository<User, UserIdentifier> repository, 
        EventProducer eventProducer, 
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

    public async Task<RegisterUserResponse> ExecuteAsync(
        UseCaseExecutionContext context, RegisterUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var id = _identifierGenerator.Generate();
        var user = new User(id, request.Nickname);

        try
        {
            await RunTransactionAsync(user, cancellationToken);
        }
        catch (DuplicateKeyStorageException ex)
        {
            return RegisterUserResponse.CreateUnknownFailure(ex.Message);
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
