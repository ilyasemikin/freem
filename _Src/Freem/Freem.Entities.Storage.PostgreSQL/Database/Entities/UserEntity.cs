namespace Freem.Entities.Storage.PostgreSQL.Database.Entities;

internal class UserEntity
{
    public required string Id { get; init; }
    
    public required string Nickname {get; init; }
}
