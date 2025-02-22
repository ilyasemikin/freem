using Freem.Entities.Models.Users;

namespace Freem.Web.Api.Public.Contracts.Users.LoginPassword;

public class RegisterPasswordCredentialsRequest
{
    public Login Login { get; }
    public Password Password { get; }

    public RegisterPasswordCredentialsRequest(Login login, Password password)
    {
        ArgumentNullException.ThrowIfNull(login);
        ArgumentNullException.ThrowIfNull(password);
        
        Login = login;
        Password = password;
    }
}