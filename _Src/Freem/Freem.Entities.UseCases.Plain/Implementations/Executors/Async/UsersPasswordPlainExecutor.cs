using Freem.Entities.UseCases.Contracts.Users.Password.Login;
using Freem.Entities.UseCases.Contracts.Users.Password.Register;
using Freem.Entities.UseCases.Contracts.Users.Password.Update;
using Freem.Entities.UseCases.Plain.Exceptions;
using Freem.Entities.Users;
using Freem.Entities.Users.Identifiers;
using Freem.UseCases.Abstractions;

namespace Freem.Entities.UseCases.Plain.Implementations.Executors.Async;

public sealed class UsersPasswordPlainExecutor
{
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;

    public UsersPasswordPlainExecutor(IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(executor);
        
        _executor = executor;
    }

    public async Task<UserIdentifier> RegisterAsync(
        RegisterUserPasswordRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        var response = await _executor.ExecuteAsync<RegisterUserPasswordRequest, RegisterUserPasswordResponse>(UseCaseExecutionContext.Empty, request, cancellationToken);
        if (!response.Success)
            throw new PlainRequestFailedException();

        return response.UserId;
    }

    public async Task<UserIdentifier> RegisterAsync(string? login = null, CancellationToken cancellationToken = default)
    {
        var nickname = Guid.NewGuid().ToString();
        login ??= Guid.NewGuid().ToString();
        var password = Guid.NewGuid().ToString();
        
        var request = new RegisterUserPasswordRequest(nickname, login, password);
        return await RegisterAsync(request, cancellationToken);
    }

    public async Task<UserTokens> LoginAsync(
        LoginUserPasswordRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        var response = await _executor.ExecuteAsync<LoginUserPasswordRequest, LoginUserPasswordResponse>(UseCaseExecutionContext.Empty, request, cancellationToken);
        if (!response.Success)
            throw new PlainRequestFailedException();

        return response.Tokens;
    }

    public async Task UpdateAsync(
        UseCaseExecutionContext context, UpdateLoginCredentialsRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(request);
        
        var response = await _executor.ExecuteAsync<UpdateLoginCredentialsRequest, UpdateLoginCredentialsResponse>(context, request, cancellationToken);
        if (!response.Success)
            throw new PlainRequestFailedException();
    }
}