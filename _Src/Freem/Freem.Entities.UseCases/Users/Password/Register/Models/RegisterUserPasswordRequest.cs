using Freem.Entities.Users.Models;

namespace Freem.Entities.UseCases.Users.Password.Register.Models;

public class RegisterUserPasswordRequest
{
    public Nickname Nickname { get; }
    
    public Entities.Users.Models.Login Login { get; }
    public Entities.Users.Models.Password Password { get; }

    public RegisterUserPasswordRequest(
        Nickname nickname, 
        Entities.Users.Models.Login login, Entities.Users.Models.Password password)
    {
        ArgumentNullException.ThrowIfNull(nickname);
        ArgumentNullException.ThrowIfNull(login);
        ArgumentNullException.ThrowIfNull(password);
        
        Nickname = nickname;
        Login = login;
        Password = password;
    }
}