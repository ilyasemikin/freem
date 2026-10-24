using Freem.Domain.Users;
using Freem.Domain.Users.Models;

namespace Freem.Api.Public.Contracts.V1.Users;

/// <summary>Profile of the current user.</summary>
public sealed class UserResponse
{
    /// <summary>User id.</summary>
    public UserId UserId { get; }
    
    /// <summary>Display name.</summary>
    public Nickname Nickname { get; }

    public UserResponse(UserId userId, Nickname nickname)
    {
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(nickname);
        
        UserId = userId;
        Nickname = nickname;
    }
}