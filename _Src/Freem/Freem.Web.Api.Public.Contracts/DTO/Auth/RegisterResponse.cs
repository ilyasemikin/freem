using Freem.Entities.Users.Identifiers;

namespace Freem.Web.Api.Public.Contracts.DTO.Auth;

public sealed class RegisterResponse
{
    public UserIdentifier UserId { get; }

    public RegisterResponse(UserIdentifier userId)
    {
        ArgumentNullException.ThrowIfNull(userId);

        UserId = userId;
    }
}
