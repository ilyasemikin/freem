namespace Freem.Entities.UseCases.Users.Password.Update.Models;

public class UpdateLoginCredentialsRequest
{
    public required Entities.Users.Models.Password OldPassword { get; init; }
    public required Entities.Users.Models.Password NewPassword { get; init; }
}