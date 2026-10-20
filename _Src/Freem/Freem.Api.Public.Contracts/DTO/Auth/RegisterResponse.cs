using Freem.Domain.Users;

namespace Freem.Api.Public.Contracts.DTO.Auth;

public sealed class RegisterResponse
{
    public UserId UserId { get; }

    public RegisterResponse(UserId userId)
    {
        ArgumentNullException.ThrowIfNull(userId);

        UserId = userId;
    }
}
