using Freem.Domain.Users.Models;

namespace Freem.Api.Public.Contracts.DTO.Users;

public class RegisterUserRequest
{
    public Nickname Nickname { get; }

    public RegisterUserRequest(Nickname nickname)
    {
        ArgumentNullException.ThrowIfNull(nickname);
        
        Nickname = nickname;
    }
}
