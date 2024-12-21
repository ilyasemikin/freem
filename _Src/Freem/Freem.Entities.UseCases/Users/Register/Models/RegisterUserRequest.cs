using Freem.Entities.Users.Models;

namespace Freem.Entities.UseCases.Users.Register.Models;

public class RegisterUserRequest
{
    public required Nickname Nickname { get; init; }
    
    public required string Login { get; init; }
    public required Password Password { get; init; }
}