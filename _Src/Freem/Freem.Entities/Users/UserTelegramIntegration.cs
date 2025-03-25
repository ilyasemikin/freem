namespace Freem.Entities.Users;

public sealed class UserTelegramIntegration
{
    public string Id { get; }

    public UserTelegramIntegration(string id)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        Id = id;
    }
}