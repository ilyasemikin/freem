using Freem.Domain.Users.Models;

namespace Freem.Api.Public.Contracts.V1.Users;

/// <summary>Request body for creating the Freem user.</summary>
public class UserRegisterRequest
{
    /// <summary>Display name.</summary>
    public Nickname Nickname { get; }

    public UserRegisterRequest(Nickname nickname)
    {
        ArgumentNullException.ThrowIfNull(nickname);
        
        Nickname = nickname;
    }
}
