using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.Users;

public sealed class UserCredentials
{
    public UserIdentifier UserId { get; }

    public UserPasswordCredentials? Password { get; set; }
    
    public UserCredentials(UserIdentifier userId)
    {
        ArgumentNullException.ThrowIfNull(userId);
        
        UserId = userId;
    }
}