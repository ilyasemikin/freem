using Freem.Credentials.Password.Abstractions;
using Freem.Credentials.Password.Implementations;
using Freem.Entities.Storage.Abstractions.Repositories;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.Events.Abstractions;
using Freem.Entities.UseCases.Users.UpdateLoginCredentials.Models;
using Freem.Entities.Users;
using Freem.Entities.Users.Models;
using Freem.Locking.Abstractions;
using Freem.Storage.Abstractions.Helpers;
using Freem.Storage.Abstractions.Helpers.Extensions;

namespace Freem.Entities.UseCases.Users.UpdateLoginCredentials;

internal sealed class UpdateLoginCredentialsUseCases : IUseCase<ChangeLoginCredentialsRequest>
{
    private readonly IDistributedLocker _locker;
    private readonly IUsersRepository _repository;
    private readonly ICurrentPasswordHashAlgorithmGetter _passwordHashAlgorithmGetter;
    private readonly PasswordRawHasher _passwordRawHasher;
    private readonly ISaltGenerator _saltGenerator;
    private readonly IEventProducer _eventProducer;
    private readonly StorageTransactionRunner _transactionRunner;

    public UpdateLoginCredentialsUseCases(
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

    public async Task ExecuteAsync(
        UseCaseExecutionContext context, ChangeLoginCredentialsRequest request,
        CancellationToken cancellationToken = default)
    {
        context.ThrowsIfUnauthorized();

        var result = await _repository.FindByIdAsync(context.UserId, cancellationToken);
        if (!result.Founded)
            throw new InvalidOperationException();

        var user = result.Entity;
        if (user.PasswordCredentials is null)
            throw new InvalidOperationException();
        
        var passwordHashAlgorithm = await _passwordHashAlgorithmGetter.GetAsync(cancellationToken);
        var oldPasswordRawHash = _passwordRawHasher.Hash(
            passwordHashAlgorithm,
            request.OldPassword.AsBytes(),
            user.PasswordCredentials.PasswordHash.Salt);

        if (oldPasswordRawHash != user.PasswordCredentials.PasswordHash)
            throw new InvalidOperationException();

        var salt = _saltGenerator.Generate();
        var newPasswordRawHash = _passwordRawHasher.Hash(passwordHashAlgorithm, request.NewPassword.AsBytes(), salt);

        var newPasswordHash = new PasswordHash(passwordHashAlgorithm, newPasswordRawHash, salt);
        var credentials = new UserPasswordCredentials(user.PasswordCredentials.Login, newPasswordHash);
        user.PasswordCredentials = credentials;

        await _transactionRunner.RunAsync(async () =>
        {
            await _repository.UpdateAsync(user, cancellationToken);
            await _eventProducer.PublishAsync(eventId => user.BuildPasswordCredentialsChangedEvent(eventId), cancellationToken);
        }, cancellationToken);
    }
}