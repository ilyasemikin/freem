using Freem.Domain.Users;

namespace Freem.Api.Public.Contracts.V1.Auth;

/// <summary>Result of registering an identity.</summary>
public sealed class RegisterResponse
{
    /// <summary>Id of the created identity; it becomes the Freem user id.</summary>
    public UserId UserId { get; }

    public RegisterResponse(UserId userId)
    {
        ArgumentNullException.ThrowIfNull(userId);

        UserId = userId;
    }
}
