using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Users.Identifiers;

namespace Freem.Web.Api.Public.Client;

public sealed class CurrentUser
{
    public UserIdentifier? UserId { get; private set; }

    public bool TryGet([NotNullWhen(true)] out UserIdentifier? userId)
    {
        userId = UserId;
        return userId is not null;
    }

    public void Set(UserIdentifier userId)
    {
        ArgumentNullException.ThrowIfNull(userId);
        
        UserId = userId;
    }

    public void Clean()
    {
        UserId = null;
    }
}
