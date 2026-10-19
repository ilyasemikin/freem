using Freem.Domain.Users.Identifiers;
using Freem.Domain.Users.Models;

namespace Freem.Web.Api.Public.Contracts.DTO.Users;

public sealed class MeResponse
{
    public UserIdentifier UserId { get; }
    
    public Nickname Nickname { get; }

    public MeResponse(UserIdentifier userId, Nickname nickname)
    {
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(nickname);
        
        UserId = userId;
        Nickname = nickname;
    }
}