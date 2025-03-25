using Freem.Entities.Users;

namespace Freem.Web.Api.Public.Contracts.Users.Tokens;

public sealed class RefreshTokensResponse
{
    public UserTokens Tokens { get; }
    
    public RefreshTokensResponse(UserTokens tokens)
    {
        ArgumentNullException.ThrowIfNull(tokens);
        
        Tokens = tokens;
    }
}