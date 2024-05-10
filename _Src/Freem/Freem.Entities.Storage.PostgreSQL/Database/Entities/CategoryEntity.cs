namespace Freem.Entities.Storage.PostgreSQL.Database.Entities;

internal class CategoryEntity
{
    public required string Id { get; init; }
    public required string UserId { get; init; }
    
    public string? Name { get; set; }

    public UserEntity? User { get; set; }
    public IList<TagEntity>? Tags { get; set; }
}
