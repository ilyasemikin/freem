using Freem.Domain.Users;
using Freem.Domain.Users.Models;

namespace Freem.Api.Public.Contracts.DTO.Users;

public sealed class MeResponse
{
    public UserId UserId { get; }
    
    public Nickname Nickname { get; }

    public MeResponse(UserId userId, Nickname nickname)
    {
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(nickname);
        
        UserId = userId;
        Nickname = nickname;
    }
}