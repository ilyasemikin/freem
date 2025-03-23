using Freem.Entities.Users;

namespace Freem.Web.Api.Public.Contracts.Users.LoginPassword;

public sealed class LoginPasswordCredentialsResponse
{
    public UserTokens Tokens { get; }
    
    public LoginPasswordCredentialsResponse(UserTokens tokens)
    {
        ArgumentNullException.ThrowIfNull(tokens);
        
        Tokens = tokens;
    }
}