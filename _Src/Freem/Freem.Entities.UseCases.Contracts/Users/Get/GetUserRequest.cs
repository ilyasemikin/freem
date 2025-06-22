using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.UseCases.Contracts.Users.Get;

public sealed class GetUserRequest
{
    public static GetUserRequest Instance { get; } = new();
}