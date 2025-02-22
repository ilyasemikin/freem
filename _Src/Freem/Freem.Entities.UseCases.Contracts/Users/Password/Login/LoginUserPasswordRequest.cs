namespace Freem.Entities.UseCases.Contracts.Users.Password.Login;

public sealed class LoginUserPasswordRequest
{
    public Models.Users.Login Login { get; }
    public Models.Users.Password Password { get; }

    public LoginUserPasswordRequest(Models.Users.Login login, Models.Users.Password password)
    {
        ArgumentNullException.ThrowIfNull(login);
        ArgumentNullException.ThrowIfNull(password);
        
        Login = login;
        Password = password;
    }
}