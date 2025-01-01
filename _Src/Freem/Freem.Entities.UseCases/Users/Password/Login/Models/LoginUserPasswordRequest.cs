namespace Freem.Entities.UseCases.Users.Password.Login.Models;

public sealed class LoginUserPasswordRequest
{
    public required Entities.Users.Models.Login Login { get; init; }
    public required Entities.Users.Models.Password Password { get; init; }
}