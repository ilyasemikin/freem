using Freem.Domain.Users.Identifiers;

namespace Freem.Web.Api.Public.Contracts.DTO.Users;

public sealed class RegisterUserResponse
{
    public UserIdentifier UserId { get; }

    public RegisterUserResponse(UserIdentifier userId)
    {
        ArgumentNullException.ThrowIfNull(userId);
        
        UserId = userId;
    }
}
