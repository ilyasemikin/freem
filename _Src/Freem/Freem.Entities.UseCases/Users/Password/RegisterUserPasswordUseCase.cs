﻿using Freem.Credentials.Password.Abstractions;
using Freem.Credentials.Password.Implementations;
using Freem.Entities.Events.Producer.Implementations;
using Freem.Entities.Storage.Abstractions.Base.Write;
using Freem.Entities.Storage.Abstractions.Exceptions;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Contracts.Users.Password.Register;
using Freem.Entities.Users;
using Freem.Entities.Users.Identifiers;
using Freem.Entities.Users.Models;
using Freem.Identifiers.Abstractions.Generators;
using Freem.Storage.Abstractions.Helpers;
using Freem.Storage.Abstractions.Helpers.Extensions;

namespace Freem.Entities.UseCases.Users.Password;

internal sealed class RegisterUserPasswordUseCase 
    : IEntitiesUseCase<RegisterUserPasswordRequest, RegisterUserPasswordResponse, RegisterUserPasswordErrorCode>
{
    private readonly IIdentifierGenerator<UserIdentifier> _identifierGenerator;
    private readonly ICreateRepository<User, UserIdentifier> _repository;
    private readonly ICurrentPasswordHashAlgorithmGetter _passwordHashAlgorithmGetter;
    private readonly PasswordRawHasher _passwordRawHasher;
    private readonly ISaltGenerator _saltGenerator;
    private readonly EventProducer _eventProducer;
    private readonly StorageTransactionRunner _transactionRunner;

    public RegisterUserPasswordUseCase(
        IIdentifierGenerator<UserIdentifier> identifierGenerator, 
        ICreateRepository<User, UserIdentifier> repository, 
        ICurrentPasswordHashAlgorithmGetter passwordHashAlgorithmGetter, 
        PasswordRawHasher passwordRawHasher, 
        ISaltGenerator saltGenerator,
        EventProducer eventProducer, 
        StorageTransactionRunner transactionRunner)
    {
        ArgumentNullException.ThrowIfNull(identifierGenerator);
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(passwordHashAlgorithmGetter);
        ArgumentNullException.ThrowIfNull(passwordRawHasher);
        ArgumentNullException.ThrowIfNull(saltGenerator);
        ArgumentNullException.ThrowIfNull(eventProducer);
        ArgumentNullException.ThrowIfNull(transactionRunner);
        
        _identifierGenerator = identifierGenerator;
        _repository = repository;
        _passwordHashAlgorithmGetter = passwordHashAlgorithmGetter;
        _passwordRawHasher = passwordRawHasher;
        _saltGenerator = saltGenerator;
        _eventProducer = eventProducer;
        _transactionRunner = transactionRunner;
    }

    public async Task<RegisterUserPasswordResponse> ExecuteAsync(
        UseCaseExecutionContext context, RegisterUserPasswordRequest request,
        CancellationToken cancellationToken = default)
    {
        var id = _identifierGenerator.Generate();

        var passwordHashAlgorithm = await _passwordHashAlgorithmGetter.GetAsync(cancellationToken);
        var saltRawHash = _saltGenerator.Generate();
        var passwordRawHash = _passwordRawHasher.Hash(passwordHashAlgorithm, request.Password.AsBytes(), saltRawHash);
        
        var passwordHash = new PasswordHash(passwordHashAlgorithm, passwordRawHash, saltRawHash);
        var credentials = new UserPasswordCredentials(request.Login, passwordHash);

        var user = new User(id, request.Nickname)
        {
            PasswordCredentials = credentials
        };

        try
        {
            await RunTransactionAsync(user, cancellationToken);
        }
        catch (DuplicateKeyStorageException ex)
        {
            return ProcessDuplicateKeyStorageException(ex);
        }

        return RegisterUserPasswordResponse.CreateSuccess(id);
    }

    private async Task RunTransactionAsync(User user, CancellationToken cancellationToken = default)
    {
        await _transactionRunner.RunAsync(async () =>
        {
            await _repository.CreateAsync(user, cancellationToken);
            await _eventProducer.PublishAsync(user.BuildRegisteredEvent, cancellationToken);
            await _eventProducer.PublishAsync(user.BuildPasswordCredentialsChangedEvent, cancellationToken);
        }, cancellationToken);
    }

    private static RegisterUserPasswordResponse ProcessDuplicateKeyStorageException(DuplicateKeyStorageException ex)
    {
        return ex.Code switch
        {
            DuplicateKeyStorageException.ErrorCode.DuplicateUserLogin => RegisterUserPasswordResponse.CreateFailure(RegisterUserPasswordErrorCode.LoginAlreadyUsed),
            _ => RegisterUserPasswordResponse.CreateFailure(RegisterUserPasswordErrorCode.UnknownError, ex.Message)
        };
    }
}