using Freem.Entities.UseCases.Contracts.Users.Tokens.Refresh;
using Freem.Entities.UseCases.Plain.Exceptions;
using Freem.UseCases.Abstractions;

namespace Freem.Entities.UseCases.Plain.Implementations.Executors.Async;

public sealed class UsersTokensPlainExecutor
{
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;

    public UsersTokensPlainExecutor(IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(executor);
        
        _executor = executor;
    }

    public async Task<Tokens> RefreshAsync(
        UseCaseExecutionContext context, string refreshToken,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(refreshToken);
        
        var request = new RefreshUserAccessTokenRequest(refreshToken);
        var response = await _executor.ExecuteAsync<RefreshUserAccessTokenRequest, RefreshUserAccessTokenResponse>(context, request, cancellationToken);
        if (!response.Success)
            throw new PlainRequestFailedException();
        
        return new Tokens(response.AccessToken, response.RefreshToken);
    }

    public sealed class Tokens
    {
        public string AccessToken { get; }
        public string RefreshToken { get; }
        
        public Tokens(string accessToken, string refreshToken)
        {
            ArgumentException.ThrowIfNullOrEmpty(accessToken);
            ArgumentException.ThrowIfNullOrEmpty(refreshToken);
            
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}