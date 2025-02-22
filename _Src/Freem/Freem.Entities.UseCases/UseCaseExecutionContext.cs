using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Identifiers;
using Freem.Entities.UseCases.Exceptions;

namespace Freem.Entities.UseCases;

public sealed class UseCaseExecutionContext
{
    public static UseCaseExecutionContext Empty { get; } = new();
    
    public UserIdentifier? UserId { get; }

    public UseCaseExecutionContext(UserIdentifier? userId = null)
    {
        UserId = userId;
    }
    
    [MemberNotNull(nameof(UserId))]
    public void ThrowsIfUnauthorized()
    {
        if (UserId is null)
            throw new UnauthorizedException(this);
    }
}