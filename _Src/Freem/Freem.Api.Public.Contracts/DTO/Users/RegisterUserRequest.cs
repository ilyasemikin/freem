using Freem.Domain.Users.Models;

namespace Freem.Api.Public.Contracts.DTO.Users;

/// <summary>Request body for creating the Freem user.</summary>
public class RegisterUserRequest
{
    /// <summary>Display name.</summary>
    public Nickname Nickname { get; }

    public RegisterUserRequest(Nickname nickname)
    {
        ArgumentNullException.ThrowIfNull(nickname);
        
        Nickname = nickname;
    }
}
