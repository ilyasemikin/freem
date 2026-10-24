using Freem.Domain.Entities.Users;

namespace Freem.Domain.UseCases.Contracts.Users.Get;

public sealed class GetUserResponse
{
    public User User { get; }

    public GetUserResponse(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        User = user;
    }
}