using System.Diagnostics.CodeAnalysis;
using Freem.Entities.UseCases.Abstractions.Exceptions;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.UseCases.Abstractions.Context;

public sealed class UseCaseExecutionContext
{
    public UserIdentifier? UserId { get; init; }

    [MemberNotNull(nameof(UserId))]
    public void ThrowsIfUnauthorized()
    {
        if (UserId is null)
            throw new UnauthorizedException();
    }
}