namespace Freem.Entities.UseCases.Contracts.Users.Password.Login;

public sealed class LoginUserPasswordRequest
{
    public Entities.Users.Models.Login Login { get; }
    public Entities.Users.Models.Password Password { get; }

    public LoginUserPasswordRequest(Entities.Users.Models.Login login, Entities.Users.Models.Password password)
    {
        ArgumentNullException.ThrowIfNull(login);
        ArgumentNullException.ThrowIfNull(password);
        
        Login = login;
        Password = password;
    }
}