using Freem.Entities.UseCases.Contracts.Users.Tokens.Refresh;
using Freem.Entities.UseCases.Plain.Exceptions;
using Freem.Entities.Users;
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

    public async Task<UserTokens> RefreshAsync(
        UseCaseExecutionContext context, string refreshToken,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(refreshToken);
        
        var request = new RefreshUserAccessTokenRequest(refreshToken);
        var response = await _executor.ExecuteAsync<RefreshUserAccessTokenRequest, RefreshUserAccessTokenResponse>(context, request, cancellationToken);
        if (!response.Success)
            throw new PlainRequestFailedException();
        
        return response.Tokens;
    }
}