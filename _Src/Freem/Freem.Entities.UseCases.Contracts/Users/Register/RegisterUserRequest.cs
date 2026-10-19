using Freem.Domain.Users.Models;

namespace Freem.Entities.UseCases.Contracts.Users.Register;

public class RegisterUserRequest
{
    public Nickname Nickname { get; }

    public RegisterUserRequest(Nickname nickname)
    {
        ArgumentNullException.ThrowIfNull(nickname);
        
        Nickname = nickname;
    }
}
