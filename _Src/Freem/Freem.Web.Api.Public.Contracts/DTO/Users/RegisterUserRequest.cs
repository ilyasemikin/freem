using Freem.Entities.Users.Models;

namespace Freem.Web.Api.Public.Contracts.DTO.Users;

public class RegisterUserRequest
{
    public Nickname Nickname { get; }

    public RegisterUserRequest(Nickname nickname)
    {
        ArgumentNullException.ThrowIfNull(nickname);
        
        Nickname = nickname;
    }
}
