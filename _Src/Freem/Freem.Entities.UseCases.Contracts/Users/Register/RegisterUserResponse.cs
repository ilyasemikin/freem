using Freem.Domain.Users;

namespace Freem.Entities.UseCases.Contracts.Users.Register;

public sealed class RegisterUserResponse
{
    public UserId UserId { get; }

    public RegisterUserResponse(UserId userId)
    {
        ArgumentNullException.ThrowIfNull(userId);

        UserId = userId;
    }
}