using Freem.Domain.Entities.Users;
using Freem.Domain.Storage.Repositories.Users;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Users.Register;
using Freem.Domain.UseCases.Implementations.Events;
using Freem.Storage.Transactions.Implementations;
using Freem.Storage.Transactions.Implementations.Extensions;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Domain.UseCases.Implementations.Users;

internal sealed class RegisterUserUseCase 
    : IRequestHandler<HandlerContext, RegisterUserRequest, Result<RegisterUserResponse, Error>>
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

    public async Task<Result<RegisterUserResponse, Error>> HandleAsync(
        HandlerContext context, RegisterUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var id = context.UserId;
        var user = new User(id, request.Nickname);

        var writeResult = await RunTransactionAsync(user, cancellationToken);
        if (!writeResult.Success)
            return writeResult.Error;

        return new RegisterUserResponse(id);
    }

    private async Task<Result<Error>> RunTransactionAsync(User user, CancellationToken cancellationToken = default)
    {
        return await _transactionRunner.RunAsync(async () =>
        {
            var repositoryResult = await _repository.CreateAsync(user, cancellationToken);
            if (!repositoryResult.Success)
                return repositoryResult;

            return await _eventProducer.PublishAsync(user.Id, user.BuildRegisteredEvent, cancellationToken);
        }, cancellationToken);
    }
}
