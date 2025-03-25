using Freem.Entities.Users.Models;

namespace Freem.Web.Api.Public.Contracts.Users.LoginPassword;

public sealed class LoginPasswordCredentialsRequest
{
    public Login Login { get; }
    public Password Password { get; }
    
    public LoginPasswordCredentialsRequest(Login login, Password password)
    {
        ArgumentNullException.ThrowIfNull(login);
        ArgumentNullException.ThrowIfNull(password);
        
        Login = login;
        Password = password;
    }
}