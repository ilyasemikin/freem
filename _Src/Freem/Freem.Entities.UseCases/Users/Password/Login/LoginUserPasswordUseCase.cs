using Freem.Credentials.Password.Implementations;
using Freem.Entities.Storage.Abstractions.Repositories;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.Users.Password.Login.Models;
using Freem.Tokens.JWT.Implementations.AccessTokens;
using Freem.Tokens.JWT.Implementations.RefreshTokens;

namespace Freem.Entities.UseCases.Users.Password.Login;

public sealed class LoginUserPasswordUseCase : IUseCase<LoginUserPasswordRequest, LoginUserPasswordResponse>
{
    private readonly IUsersRepository _repository;
    private readonly PasswordRawHasher _passwordRawHasher;
    private readonly AccessTokenGenerator _accessTokenGenerator;
    private readonly RefreshTokenGenerator _refreshTokenGenerator;

    public LoginUserPasswordUseCase(
        IUsersRepository repository,
        PasswordRawHasher passwordRawHasher,
        AccessTokenGenerator accessTokenGenerator, 
        RefreshTokenGenerator refreshTokenGenerator)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(passwordRawHasher);
        ArgumentNullException.ThrowIfNull(accessTokenGenerator);
        ArgumentNullException.ThrowIfNull(refreshTokenGenerator);

        _repository = repository;
        _passwordRawHasher = passwordRawHasher;
        _accessTokenGenerator = accessTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
    }

    public async Task<LoginUserPasswordResponse> ExecuteAsync(
        UseCaseExecutionContext context, LoginUserPasswordRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _repository.FindByLoginAsync(request.Login, cancellationToken);
        if (!result.Founded)
            return LoginUserPasswordResponse.Failure();

        var user = result.Entity;
        if (user.PasswordCredentials is null)
            return LoginUserPasswordResponse.Failure();

        var actualPassword = user.PasswordCredentials.PasswordHash;
        var hash = _passwordRawHasher.Hash(actualPassword.Algorithm, request.Password.AsBytes(), actualPassword.Salt);
        
        if (hash != actualPassword)
            return LoginUserPasswordResponse.Failure();

        var accessToken = _accessTokenGenerator.Generate(user);
        var refreshToken = _refreshTokenGenerator.Generate(user);
        
        return LoginUserPasswordResponse.Authorize(accessToken, refreshToken);
    }
}