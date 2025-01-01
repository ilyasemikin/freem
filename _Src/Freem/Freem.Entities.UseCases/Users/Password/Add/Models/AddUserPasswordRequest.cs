namespace Freem.Entities.UseCases.Users.Password.Add.Models;

public sealed class AddUserPasswordRequest
{
    public required Entities.Users.Models.Login Login { get; init; }
    public required Entities.Users.Models.Password Password { get; init; }
}