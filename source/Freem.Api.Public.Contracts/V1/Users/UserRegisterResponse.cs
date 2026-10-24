using Freem.Domain.Users;

namespace Freem.Api.Public.Contracts.V1.Users;

/// <summary>Result of registering a user.</summary>
public sealed class UserRegisterResponse
{
    /// <summary>Id of the created user.</summary>
    public UserId UserId { get; }

    public UserRegisterResponse(UserId userId)
    {
        ArgumentNullException.ThrowIfNull(userId);
        
        UserId = userId;
    }
}
