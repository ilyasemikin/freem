using Freem.Entities.Models.Users;
using Freem.Entities.Users.Models;

namespace Freem.Entities.UseCases.Contracts.Users.Password.Register;

public class RegisterUserPasswordRequest
{
    public Nickname Nickname { get; }
    
    public Models.Users.Login Login { get; }
    public Models.Users.Password Password { get; }

    public RegisterUserPasswordRequest(
        Nickname nickname, 
        Models.Users.Login login, Models.Users.Password password)
    {
        ArgumentNullException.ThrowIfNull(nickname);
        ArgumentNullException.ThrowIfNull(login);
        ArgumentNullException.ThrowIfNull(password);
        
        Nickname = nickname;
        Login = login;
        Password = password;
    }
}