using Freem.Domain.Users;

namespace Freem.Api.Public.Contracts.DTO.Users;

public sealed class RegisterUserResponse
{
    public UserId UserId { get; }

    public RegisterUserResponse(UserId userId)
    {
        ArgumentNullException.ThrowIfNull(userId);
        
        UserId = userId;
    }
}
