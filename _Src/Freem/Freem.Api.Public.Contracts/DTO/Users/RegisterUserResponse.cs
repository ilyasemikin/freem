using Freem.Domain.Users;

namespace Freem.Api.Public.Contracts.DTO.Users;

/// <summary>Result of registering a user.</summary>
public sealed class RegisterUserResponse
{
    /// <summary>Id of the created user.</summary>
    public UserId UserId { get; }

    public RegisterUserResponse(UserId userId)
    {
        ArgumentNullException.ThrowIfNull(userId);
        
        UserId = userId;
    }
}
