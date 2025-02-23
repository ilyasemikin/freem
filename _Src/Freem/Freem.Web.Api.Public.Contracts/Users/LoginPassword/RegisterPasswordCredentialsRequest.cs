using Freem.Entities.Models.Users;

namespace Freem.Web.Api.Public.Contracts.Users.LoginPassword;

public class RegisterPasswordCredentialsRequest
{
    public Nickname Nickname { get; }
    public Login Login { get; }
    public Password Password { get; }

    public RegisterPasswordCredentialsRequest(Nickname nickname, Login login, Password password)
    {
        ArgumentNullException.ThrowIfNull(nickname);
        ArgumentNullException.ThrowIfNull(login);
        ArgumentNullException.ThrowIfNull(password);
        
        Nickname = nickname;
        Login = login;
        Password = password;
    }
}