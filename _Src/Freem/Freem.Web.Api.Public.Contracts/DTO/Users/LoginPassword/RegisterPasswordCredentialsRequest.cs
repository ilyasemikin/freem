using Freem.Entities.Users.Models;

namespace Freem.Web.Api.Public.Contracts.DTO.Users.LoginPassword;

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