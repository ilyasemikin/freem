using Freem.Credentials.Password.Abstractions;
using Freem.Credentials.Password.Implementations;
using Freem.Entities.Storage.Abstractions.Repositories;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.Events.Abstractions;
using Freem.Entities.UseCases.Users.Password.Add.Models;
using Freem.Entities.Users;
using Freem.Entities.Users.Models;
using Freem.Locking.Abstractions;
using Freem.Locking.Abstractions.Extensions;
using Freem.Storage.Abstractions.Helpers;
using Freem.Storage.Abstractions.Helpers.Extensions;

namespace Freem.Entities.UseCases.Users.Password.Add;

internal sealed class AddUserPasswordUseCase : IUseCase<AddUserPasswordRequest, AddUserPasswordResponse>
{
    private readonly IDistributedLocker _locker;
    private readonly IUsersRepository _repository;
    private readonly ICurrentPasswordHashAlgorithmGetter _passwordHashAlgorithmGetter;
    private readonly PasswordRawHasher _passwordRawHasher;
    private readonly ISaltGenerator _saltGenerator;
    private readonly IEventProducer _eventProducer;
    private readonly StorageTransactionRunner _transactionRunner;

    public AddUserPasswordUseCase(
        IDistributedLocker locker, 
        IUsersRepository repository, 
        ICurrentPasswordHashAlgorithmGetter passwordHashAlgorithmGetter, 
        PasswordRawHasher passwordRawHasher, 
        ISaltGenerator saltGenerator, 
        IEventProducer eventProducer, 
        StorageTransactionRunner transactionRunner)
    {
        ArgumentNullException.ThrowIfNull(locker);
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(passwordHashAlgorithmGetter);
        ArgumentNullException.ThrowIfNull(passwordRawHasher);
        ArgumentNullException.ThrowIfNull(saltGenerator);
        ArgumentNullException.ThrowIfNull(eventProducer);
        ArgumentNullException.ThrowIfNull(transactionRunner);
        
        _locker = locker;
        _repository = repository;
        _passwordHashAlgorithmGetter = passwordHashAlgorithmGetter;
        _passwordRawHasher = passwordRawHasher;
        _saltGenerator = saltGenerator;
        _eventProducer = eventProducer;
        _transactionRunner = transactionRunner;
    }

    public async Task<AddUserPasswordResponse> ExecuteAsync(
        UseCaseExecutionContext context, AddUserPasswordRequest request,
        CancellationToken cancellationToken = default)
    {
        context.ThrowsIfUnauthorized();

        await using var @lock = await _locker.LockAsync(Lock.Prefix + context.UserId, cancellationToken);

        var result = await _repository.FindByIdAsync(context.UserId,cancellationToken);
        if (!result.Founded)
            return AddUserPasswordResponse.Failed();

        var user = result.Entity;
        if (user.PasswordCredentials is not null)
            return AddUserPasswordResponse.Failed();

        var passwordHashAlgorithm = await _passwordHashAlgorithmGetter.GetAsync(cancellationToken);
        var salt = _saltGenerator.Generate();
        var passwordRawHash = _passwordRawHasher.Hash(passwordHashAlgorithm, request.Password.AsBytes(), salt);

        var password = new PasswordHash(passwordHashAlgorithm, passwordRawHash, salt);
        var credentials = new UserPasswordCredentials(request.Login, password);
        user.PasswordCredentials = credentials;

        await _transactionRunner.RunAsync(async () =>
        {
            await _repository.UpdateAsync(user, cancellationToken);
            await _eventProducer.PublishAsync(eventId => user.BuildPasswordCredentialsChangedEvent(eventId),cancellationToken);
        }, cancellationToken);
        
        return AddUserPasswordResponse.Added();
    }
}