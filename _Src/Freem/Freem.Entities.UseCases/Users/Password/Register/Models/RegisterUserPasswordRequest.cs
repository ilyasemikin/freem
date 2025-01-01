using Freem.Entities.Users.Models;

namespace Freem.Entities.UseCases.Users.Password.Register.Models;

public class RegisterUserPasswordRequest
{
    public required Nickname Nickname { get; init; }
    
    public required Entities.Users.Models.Login Login { get; init; }
    public required Entities.Users.Models.Password Password { get; init; }
}