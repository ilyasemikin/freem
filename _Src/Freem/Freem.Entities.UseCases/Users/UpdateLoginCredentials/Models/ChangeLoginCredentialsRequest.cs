using Freem.Entities.Users.Models;

namespace Freem.Entities.UseCases.Users.UpdateLoginCredentials.Models;

public class ChangeLoginCredentialsRequest
{
    public required Password OldPassword { get; init; }
    public required Password NewPassword { get; init; }
}