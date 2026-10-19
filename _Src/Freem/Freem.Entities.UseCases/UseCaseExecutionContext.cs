using System.Diagnostics.CodeAnalysis;
using Freem.Domain.Users;
using Freem.Entities.UseCases.Exceptions;

namespace Freem.Entities.UseCases;

public sealed class UseCaseExecutionContext
{
    public static UseCaseExecutionContext Empty { get; } = new();
    
    public UserId? UserId { get; }

    public UseCaseExecutionContext(UserId? userId = null)
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